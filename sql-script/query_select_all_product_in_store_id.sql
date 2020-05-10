select product1."Id", product1."ProductName", ps."Sku", product1."OptionName", 
product1."ValueName", ps."Price", product1."ImageThumb", product1."ShortDescription", product1."LongDescription"
from "Product"."ProductSKUs" ps
inner join "Product"."ProductSKUValues" psvalue on ps."SkuId" = psvalue."SkuId" 
inner join
(select p."Id", p."ProductName", po."OptionId" , po."OptionName", povalue."ValueName", pod."ImageThumb", pod."ShortDescription", pod."LongDescription" from "Product"."Products" p
inner join "Product"."ProductOptions" po on po."ProductId" = p."Id" 
inner join "Product"."ProductOptionValues" povalue on povalue."OptionId" = po."OptionId"
inner join "Product"."ProductDetails" pod on pod."ProductId" = p."Id" 
where p."StoreId" = 'b4d32aca-665d-4253-83a5-6f6a8b7acade') as product1
on product1."OptionId" = psvalue."OptionId" 
	
-- "Product"."ProductOptions" po on po."OptionId" = psvalue."OptionId"
