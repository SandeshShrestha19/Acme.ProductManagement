abp install-libs

cd src/Acme.ProductManagement.DbMigrator && dotnet run && cd -


cd src/Acme.ProductManagement.HttpApi.Host && dotnet dev-certs https -v -ep openiddict.pfx -p config.auth_server_default_pass_phrase 



exit 0