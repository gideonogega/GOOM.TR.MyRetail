package com.goom.tr.myretail;

import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.context.annotation.Bean;
import org.springframework.context.annotation.Configuration;

import com.goom.tr.myretail.repos.IPriceRepo;
import com.goom.tr.myretail.repos.IProductRepo;
import com.goom.tr.myretail.repos.PriceRepo;
import com.goom.tr.myretail.repos.ProductRepo;
import com.goom.tr.myretail.repos.mongo.PricesRepository;
import com.goom.tr.myretail.services.IProductService;
import com.goom.tr.myretail.services.ProductService;

import springfox.documentation.builders.PathSelectors;
import springfox.documentation.builders.RequestHandlerSelectors;
import springfox.documentation.spi.DocumentationType;
import springfox.documentation.spring.web.plugins.Docket;

@Configuration
public class MyRetailApplicationConfig {

	@Autowired
	private PricesRepository repository;

	@Bean
	public Docket api() {
		return new Docket(DocumentationType.SWAGGER_2).select()
				.apis(RequestHandlerSelectors.basePackage(MyRetailApplicationConfig.class.getPackageName()))
				.paths(PathSelectors.any()).build();
	}

	@Bean
	public IProductService productService() {
		return new ProductService(productRepo(), priceRepo());
	}

	@Bean
	public IProductRepo productRepo() {
		return new ProductRepo();
	}

	@Bean
	public IPriceRepo priceRepo() {
		return new PriceRepo(repository);
	}
}
