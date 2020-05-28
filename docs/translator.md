# Translator Module

## Contributes 

* [Database Localization and Internationalization](https://www.soluling.com/Help/Database/Index.htm)
* [Database Internationalization(I18N)/Localization(L10N) design patterns](https://medium.com/walkin/database-internationalization-i18n-localization-l10n-design-patterns-94ff372375c6)

## Features

- Translate for data table
- Translate for key translate general app
- Support Multi-Language Eng, Vi, Fr,...
- Design EX:

```
	PRODUCTS (
		id
		price
		created_at
	)

	LANGUAGES (
		id
		title
	)

	TRANSLATIONS (
		id (// id of translation, UNIQUE)
		language_id (// id of desired language)
		table_name (// any table, in this case PRODUCTS)
		item_id (// id of item in PRODUCTS)
		field_name (// fields to be translated)
		translation (// translation text goes here)
	)

```