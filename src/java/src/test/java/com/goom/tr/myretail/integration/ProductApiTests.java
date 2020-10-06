package com.goom.tr.myretail.integration;

import static org.junit.jupiter.api.Assertions.assertEquals;
import static org.junit.jupiter.api.Assertions.assertNotNull;
import static org.junit.jupiter.api.Assertions.assertNull;
import static org.springframework.test.web.servlet.request.MockMvcRequestBuilders.get;
import static org.springframework.test.web.servlet.result.MockMvcResultMatchers.status;

import org.junit.jupiter.api.Test;
import org.springframework.beans.factory.annotation.Autowired;
import org.springframework.boot.test.autoconfigure.web.servlet.AutoConfigureMockMvc;
import org.springframework.boot.test.context.SpringBootTest;
import org.springframework.test.web.servlet.MockMvc;
import org.springframework.test.web.servlet.MvcResult;

import com.fasterxml.jackson.databind.ObjectMapper;
import com.goom.tr.myretail.models.Product;
import com.goom.tr.myretail.repos.mongo.PricesRepository;
import com.goom.tr.myretail.repos.mongo.models.DbPrice;

@SpringBootTest
@AutoConfigureMockMvc
class ProductsApiTests {

	@Autowired
	private MockMvc mockMvc;
	
	@Autowired
	private ObjectMapper objectMapper;
	
	@Autowired
	private PricesRepository pricesRepository;

	@Test
	void Get_InvalidProductId_ReturnsNull() throws Exception {
		MvcResult result = mockMvc.perform(get("/products/-1"))
		.andExpect(status().isOk())
		.andReturn();
		
		String contentAsString = result.getResponse().getContentAsString();
		assertEquals(contentAsString, "");
	}

	@Test
	void Get_ProductWithoutPrice_ReturnsPartialResponse() throws Exception {
		MvcResult result = mockMvc.perform(get("/products/13860428"))
		.andExpect(status().isOk())
		.andReturn();
		
		String contentAsString = result.getResponse().getContentAsString();

		Product response = objectMapper.readValue(contentAsString, Product.class);
		assertNotNull(response);
		assertEquals(response.Id, 13860428);
		assertEquals(response.Name, "The Big Lebowski (Blu-ray)");
		assertNull(response.CurrentPrice);
	}
	
	@Test
	void Get_ProductWithPrice_ReturnsFullResponse() throws Exception {
		// Arrange
		DbPrice dbPrice = pricesRepository.findByProductId(54456119);
		if(dbPrice == null) {
			dbPrice = new DbPrice();
			dbPrice.productId = 54456119;
			dbPrice.currencyCode = "USD";
			dbPrice.value = 6.99f;
			pricesRepository.save(dbPrice);
		}
		
		// Act
		
		MvcResult result = mockMvc.perform(get("/products/54456119"))
		.andExpect(status().isOk())
		.andReturn();
		
		// Assert
		String contentAsString = result.getResponse().getContentAsString();

		Product response = objectMapper.readValue(contentAsString, Product.class);
		assertNotNull(response);
		assertEquals(response.Id, 54456119);
		assertEquals(response.Name, "Creamy Peanut Butter 40oz - Good &#38; Gather&#8482;");
		assertEquals(response.CurrentPrice.CurrencyCode, "USD");
		assertEquals(response.CurrentPrice.Value, 6.99f);
	}
}
