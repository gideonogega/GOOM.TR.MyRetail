package com.goom.tr.myretail.repos;

import com.goom.tr.myretail.models.Price;
import com.goom.tr.myretail.repos.mongo.PricesRepository;
import com.goom.tr.myretail.repos.mongo.models.DbPrice;

public class PriceRepo implements IPriceRepo {
	private final PricesRepository _pricesRepository;
	
	public PriceRepo(PricesRepository pricesRepository) {
		_pricesRepository = pricesRepository;
	}
	
	@Override
	public Price GetPriceByProductId(long productId) {
		DbPrice dbPrice = _pricesRepository.findByProductId(productId);
		if(dbPrice == null) return null;
		
		var price = new Price();
		price.CurrencyCode = dbPrice.currencyCode;
		price.Value = dbPrice.value;
		return price;
	}

	@Override
	public Price SetProductPrice(long productId, Price price) {

		DbPrice dbPrice = _pricesRepository.findByProductId(productId);
		if(dbPrice == null) { 
			dbPrice = new DbPrice(); 
			dbPrice.productId = productId;
		}
		dbPrice.currencyCode = price.CurrencyCode;
		dbPrice.value = price.Value;
		
		
		_pricesRepository.save(dbPrice);
		
		return price;
	}

	@Override
	public boolean DeleteProductPrice(long productId) {
		DbPrice dbPrice = _pricesRepository.findByProductId(productId);
		if(dbPrice == null) return false;
		
		_pricesRepository.delete(dbPrice);
		return true;
	}
}
