package com.goom.tr.myretail.services;

import org.slf4j.Logger;
import org.slf4j.LoggerFactory;

import com.goom.tr.myretail.models.*;
import com.goom.tr.myretail.repos.*;
import com.goom.tr.myretail.repos.redsky.models.ProductResponse;

public class ProductService implements IProductService {

	private Logger Log = LoggerFactory.getLogger(ProductService.class);

	private final IProductRepo _productRepo;
	private final IPriceRepo _priceRepo;

	public ProductService(IProductRepo productRepo, IPriceRepo priceRepo) {
		_productRepo = productRepo;
		_priceRepo = priceRepo;
	}

	@Override
	public Product GetProduct(long id) {

		try {
			Product product = _productRepo.GetProduct(id);
			if (product == null)
				return product;

			product.CurrentPrice = _priceRepo.GetPriceByProductId(id);
			return product;
		} catch (Exception ex) {
			Log.error(ex.getMessage(), ex);
		}
		return null;
	}

	@Override
	public Price SetProductCurrentPrice(long id, Price price) {
		try {
			return _priceRepo.SetProductPrice(id, price);
		} catch (Exception ex) {
			Log.error(ex.getMessage(), ex);
		}
		return null;
	}

	@Override
	public boolean DeleteProductCurrentPrice(long id) {
		try {
			return _priceRepo.DeleteProductPrice(id);
		} catch (Exception ex) {
			Log.error(ex.getMessage(), ex);
		}
		return false;
	}

}
