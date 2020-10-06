package com.goom.tr.myretail.repos;

import com.goom.tr.myretail.models.Price;

public interface IPriceRepo {
	Price GetPriceByProductId(long productId);

	Price SetProductPrice(long productId, Price price);

	boolean DeleteProductPrice(long productId);
}
