package com.goom.tr.myretail.repos;

import com.goom.tr.myretail.models.Product;

public interface IProductRepo {
	Product GetProduct(long id);
}
