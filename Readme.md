# StoreOrder Web API

* A project case-study for restaurent - nguyenvannam

# Contributors

* [Blog Save many to many relationship in EFCore](https://stackoverflow.com/questions/38893873/saving-many-to-many-relationship-in-entity-framework-core)
* [Modeling product variants design](https://stackoverflow.com/questions/24923469/modeling-product-variants)
* [Database HanhChinhVietNam](https://github.com/ngnam/hanhchinhvn)
* [Entity Framework](https://www.entityframeworktutorial.net/code-first/configure-many-to-many-relationship-in-code-first.aspx)
* [Entity Framework Core](https://www.learnentityframeworkcore.com/configuration/one-to-many-relationship-configuration)
* [Auto Wrappers](https://vmsdurano.com/asp-net-core-with-autowrapper-customizing-the-default-response-output/)
* [Why choose postgreSQL](https://dzone.com/articles/customizing-automatic-http-400-error-response-in-a)
* [Implement Swagger & migration from open 2.0 -> open 3.0](https://github.com/ngnam/WebApiExamples)
* Install Swashbuckle `Install-Package Swashbuckle.AspNetCore -Version 5.0.0`
* [Convert `DateTime.Now.Tick` to int ](https://stackoverflow.com/questions/2695093/how-to-maintain-precision-using-datetime-now-ticks-in-c-sharp/2695525)
* [Modeling product variants](https://help.akeneo.com/pim/serenity/articles/what-about-products-variants.html)
* http://demo.akeneo.com/
* [Logging SQL and Change-Tracking Events in EF Core](https://docs.microsoft.com/en-us/archive/msdn-magazine/2018/october/data-points-logging-sql-and-change-tracking-events-in-ef-core)
* [Add serilog](https://andrewlock.net/adding-serilog-to-the-asp-net-core-generic-host/) - `Update-Database -c AdminLogDbContext`
* [Multiple translate database](docs/translator.md)
* [Sercurity](docs/sercurity.md)
* [Heroku Build pack apt](https://elements.heroku.com/buildpacks/ivahero/heroku-buildpack-apt)

## Environment

* [Postgres 12.2](https://www.enterprisedb.com/downloads/postgres-postgresql-downloads)
* [dbeaver Managerment studio postgress DB](https://dbeaver.io/download/)

## Heroku SQL  

* show info pg:info - `pg:info -a storeorder`
* show addons using - `heroku addons -a storeorder`
* `heroku ps:scale web=0 --app myAppName `
* Turn off 
```
USAGE
  $ heroku maintenance

OPTIONS
  -a, --app=app        (required) app to run command against
  -r, --remote=remote  git remote of app to use

COMMANDS
  maintenance:off  take the app out of maintenance mode
  maintenance:on   put the app into maintenance mode
```

* Herku checkout 
`heroku run bash -a myApp`
`heroku buildpacks:add --index 1 heroku-community/apt`
`heroku buildpacks -a myApp`

## License

* choose license: Opensource

# Release History

* See: [Release log](/RELEASE.MD)

## You can clear the build cache for an app using the following commands:

	`$ heroku plugins:install heroku-repo`
	`$ heroku repo:purge_cache -a appname`

# Donate
If you find this project useful — or just feeling generous, consider buying me a beer or a coffee. Cheers! :beers: :coffee:
|               |               |
| ------------- |:-------------:|
|   <a href="https://www.paypal.me/ngnam39"><img src="https://github.com/ngnam/Resources/blob/master/donate_paypal.svg" height="40"></a>   | [![BMC](https://github.com/ngnam/Resources/blob/master/donate_coffee.png)](https://www.buymeacoffee.com/ngnam) |
  
Thank you!


# Get more

## StoreOrder Client Admin WebApp

* See: [Release log](docs/client-admin-web-app-readme.md)

## StoreOrder Client Android App

* See: [Release log](docs/client-android-app-readme.md)

## StoreOrder Client IOS App

* See: [Release log](docs/client-ios-app-readme.md)