SELECT p6."ProductId", p6."Price", p7."ProductName", p7."Weight", p7."Depth", p7."Height", p7."UniversalProductCode", p8."ImageHeightThumb", p8."ImageOrigin", p8."ImageThumb", p8."ImageWidthThumb", p8."LongDescription", p7."NetWeight", p6."Sku", p6."SkuId", p8."ShortDescription", (
          SELECT p."OptionId"
          FROM "Product"."ProductSKUValues" AS p
          WHERE ((p6."ProductId" = p."ProductId") AND (p6."SkuId" = p."SkuId")) AND (p."SkuId" = p6."SkuId")
          LIMIT 1) AS "OptionId", (
          SELECT p1."OptionName"
          FROM "Product"."ProductSKUValues" AS p0
          INNER JOIN "Product"."ProductOptions" AS p1 ON (p0."ProductId" = p1."ProductId") AND (p0."OptionId" = p1."OptionId")
          WHERE ((p6."ProductId" = p0."ProductId") AND (p6."SkuId" = p0."SkuId")) AND (p0."SkuId" = p6."SkuId")
          LIMIT 1) AS "OptionName", (
          SELECT p3."ValueId"
          FROM "Product"."ProductSKUValues" AS p2
          LEFT JOIN "Product"."ProductOptionValues" AS p3 ON ((p2."ProductId" = p3."ProductId") AND (p2."OptionId" = p3."OptionId")) AND (p2."ValueId" = p3."ValueId")
          WHERE ((p6."ProductId" = p2."ProductId") AND (p6."SkuId" = p2."SkuId")) AND (p2."SkuId" = p6."SkuId")
          LIMIT 1) AS "ValueId", (
          SELECT p5."ValueName"
          FROM "Product"."ProductSKUValues" AS p4
          LEFT JOIN "Product"."ProductOptionValues" AS p5 ON ((p4."ProductId" = p5."ProductId") AND (p4."OptionId" = p5."OptionId")) AND (p4."ValueId" = p5."ValueId")
          WHERE ((p6."ProductId" = p4."ProductId") AND (p6."SkuId" = p4."SkuId")) AND (p4."SkuId" = p6."SkuId")
          LIMIT 1) AS "ValueName"
      FROM "Product"."ProductSKUs" AS p6
      INNER JOIN "Product"."Products" AS p7 ON p6."ProductId" = p7."Id"
      LEFT JOIN "Product"."ProductDetails" AS p8 ON p7."Id" = p8."ProductId"
      WHERE p7."StoreId" = 'b4d32aca-665d-4253-83a5-6f6a8b7acade'
      ORDER BY (SELECT 1)
      LIMIT '10' OFFSET '0'