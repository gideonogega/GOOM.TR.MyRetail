package com.goom.tr.myretail.controllers;

import org.springframework.web.bind.annotation.DeleteMapping;
import org.springframework.web.bind.annotation.GetMapping;
import org.springframework.web.bind.annotation.PathVariable;
import org.springframework.web.bind.annotation.PutMapping;
import org.springframework.web.bind.annotation.RequestBody;
import org.springframework.web.bind.annotation.RequestMapping;
import org.springframework.web.bind.annotation.RestController;

import com.goom.tr.myretail.models.*;
import com.goom.tr.myretail.services.IProductService;

@RestController
@RequestMapping("/products")
public class ProductsController {
    private IProductService productService;

    public ProductsController(IProductService productService)
    {
        this.productService = productService;
    }

    @GetMapping("{id}")
    public Product Get(@PathVariable long id)
    {
        return productService.GetProduct(id);
    }

    @PutMapping("{id}/current_price")
    public Price SetCurrentPrice(@PathVariable long id, @RequestBody Price currentPrice)
    {
        return productService.SetProductCurrentPrice(id, currentPrice);
    }

    @DeleteMapping("{id}/current_price")
    public boolean DeleteCurrentPrice(@PathVariable long id)
    {
        return productService.DeleteProductCurrentPrice(id);
    }
}
