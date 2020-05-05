var ngnamAuth = ngnamAuth || {};
(function () {

    /* Swagger */

    ngnamAuth.swagger = ngnamAuth.swagger || {};

    ngnamAuth.swagger.addAuthToken = function () {
        var authToken = ngnamAuth.auth.getToken();
        if (!authToken) {
            return false;
        }
        var cookieAuth = new SwaggerClient.ApiKeyAuthorization(ngnamAuth.auth.tokenHeaderName, 'Bearer ' + authToken, 'header');
        swaggerUi.api.clientAuthorizations.add('bearerAuth', cookieAuth);
        return true;
    }

    ngnamAuth.swagger.addCsrfToken = function () {
        var csrfToken = ngnamAuth.security.antiForgery.getToken();
        if (!csrfToken) {
            return false;
        }
        var csrfCookieAuth = new SwaggerClient.ApiKeyAuthorization(ngnamAuth.security.antiForgery.tokenHeaderName, csrfToken, 'header');
        swaggerUi.api.clientAuthorizations.add(ngnamAuth.security.antiForgery.tokenHeaderName, csrfCookieAuth);
        return true;
    }

    ngnamAuth.swagger.login = function () {
        var usernameOrEmailAddress = window.prompt('UserName');
        if (!usernameOrEmailAddress) {
            return false;
        }
        var password = window.prompt('password');
        var xhr = new XMLHttpRequest();
        xhr.onreadystatechange = function () {
            if (xhr.readyState === XMLHttpRequest.DONE && xhr.status === 200) {
                var responseJSON = JSON.parse(xhr.responseText);
                var result = responseJSON.result;
                var expireDate = new Date(Date.now() + (result.expireInSeconds * 1000));
                ngnamAuth.auth.setToken(result.accessToken, expireDate);
                ngnamAuth.swagger.addAuthToken();
                console.log(true);
            }
        };
        xhr.open('POST', '/api/Account/login/staff', true);
        //xhr.setRequestHeader('Abp.TenantId', tenantId);
        xhr.setRequestHeader('Content-type', 'application/json');
        xhr.send("{" +
            "UserNameOrEmail:'" + usernameOrEmailAddress + "'," +
            "Password:'" + password + "'}" +
            "RememberMe:'" + true + "'}"
        );
    }

})();
