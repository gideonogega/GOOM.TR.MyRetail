package com.goom.tr.myretail.repos;

import java.util.ArrayList;
import java.util.List;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;
import org.springframework.http.converter.HttpMessageConverter;
import org.springframework.http.converter.json.MappingJackson2HttpMessageConverter;
import org.springframework.web.client.RestTemplate;

import com.fasterxml.jackson.databind.DeserializationFeature;
import com.fasterxml.jackson.databind.ObjectMapper;
import com.fasterxml.jackson.databind.PropertyNamingStrategy;
import com.goom.tr.myretail.models.Product;
import com.goom.tr.myretail.repos.redsky.models.ProductResponse;

public class ProductRepo implements IProductRepo {
	private Logger Log = LoggerFactory.getLogger(ProductRepo.class);

	private static final String[] EXCLUDES_ARRAY = { "available_to_promise_network", "taxonomy", "price", "promotion",
			"bulk_ship", "rating_and_review_reviews", "rating_and_review_statistics", "question_answer_statistics",
			"available_to_promise_network" };
	private static final String EXCLUDES = String.join(",", EXCLUDES_ARRAY);
	private static final String KEY = "candidate";

	@Override
	public Product GetProduct(long id) {
		String uri = String.format("https://redsky.target.com/v3/pdp/tcin/%s?excludes=%s&key=%s#_blank", id, EXCLUDES,
				KEY);
		RestTemplate restTemplate = restTemplate();

		try {
			ProductResponse productResponse = restTemplate.getForObject(uri, ProductResponse.class);
			if (productResponse.Product.Item.ErrorMessage == null) {
				Product product = new Product();
				product.Id = id;
				product.Name = productResponse.Product.Item.ProductDescription.Title;
				return product;
			}
		} catch (Exception ex) {
			Log.error(ex.getMessage(), ex);
		}

		return null;
	}

	private RestTemplate restTemplate() {
		RestTemplate restTemplate = new RestTemplate();
		List<HttpMessageConverter<?>> messageConverters = new ArrayList<>();
		MappingJackson2HttpMessageConverter jsonMessageConverter = new MappingJackson2HttpMessageConverter();
		jsonMessageConverter.setObjectMapper(jacksonObjectMapper());
		messageConverters.add(jsonMessageConverter);
		restTemplate.setMessageConverters(messageConverters);

		return restTemplate;
	}

	private ObjectMapper jacksonObjectMapper() {
		return new ObjectMapper().setPropertyNamingStrategy(PropertyNamingStrategy.SNAKE_CASE)
				.configure(DeserializationFeature.FAIL_ON_UNKNOWN_PROPERTIES, false);
	}
}
