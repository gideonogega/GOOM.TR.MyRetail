package com.goom.tr.myretail.repos.mongo;

import org.springframework.data.mongodb.repository.MongoRepository;

import com.goom.tr.myretail.repos.mongo.models.DbPrice;

public interface PricesRepository extends MongoRepository<DbPrice, String> {

  public DbPrice findByProductId(long productId);
}
