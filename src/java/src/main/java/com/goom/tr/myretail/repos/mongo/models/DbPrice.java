package com.goom.tr.myretail.repos.mongo.models;

import org.bson.types.ObjectId;
import org.springframework.data.annotation.Id;
import org.springframework.data.mongodb.core.index.Indexed;
import org.springframework.data.mongodb.core.mapping.Document;
import org.springframework.data.mongodb.core.mapping.Field;
import org.springframework.data.mongodb.core.mapping.FieldType;

@Document(collection = "Prices")
public class DbPrice {
	@Id
	@Field(targetType = FieldType.OBJECT_ID)
	public ObjectId id;

	@Indexed(unique=true)
	@Field("ProductId")
	public long productId;

	@Field("Value")
	public float value;

	@Field("CurrencyCode")
	public String currencyCode;
}
