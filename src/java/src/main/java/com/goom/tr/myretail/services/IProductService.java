package com.goom.tr.myretail.services;

import com.goom.tr.myretail.models.*;

public interface IProductService {
    Product GetProduct(long id);
    Price SetProductCurrentPrice(long id, Price price);
    boolean DeleteProductCurrentPrice(long id);
}
