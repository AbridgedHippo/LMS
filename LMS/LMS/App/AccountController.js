(function () {
    // main module
    var app = angular.module("LMSApp").controller("AccountController", ["$http", "$window", function ($http, $window) {
        "use strict";

        var self = this;
        self.login = {
            "UserName": "Admin",
            "Password": "Admin-1234"
        };

        self.requestLogin = function () {
            var data = "grant_type=password&username="
                + self.login.UserName
                + "&password="
                + self.login.Password;

            $http.post("/Token", data)
                .then(function (promise) {
                    self.user = promise.data.userName;
                    $window.sessionStorage.setItem("tokenKey", promise.data.access_token);
                    $window.sessionStorage.setItem("startUrl", document.location.href);
                    goTo(getStartUrl());
                });
        };

        self.logOut = function () {
            $http.post("/api/account/logout")
                .then(function (promise) {
                    console.log("Logged out!: " + promise.data);
                    var startUrl = getStartUrl();
                    $window.localStorage.clear();
                    $window.sessionStorage.clear();
                    goTo(startUrl);
                });
        };

        self.requestData = function () {
            var config = getSessionTokenConfig();

            $http.get("/api/values", config)
                .then(function (promise) {
                });
        };

        var goTo = function (targetUrl) {
            document.location.href = targetUrl;
        };
        var getSessionToken = function () {
            return $window.sessionStorage.getItem("tokenKey");
        };
        var getStartUrl = function () {
            return $window.sessionStorage.getItem("startUrl");
        };
        var getSessionTokenConfig = function () {
            var token = getSessionToken();
            var headers = {};
            if (token) {
                headers.Authorization = "Bearer " + token;
            }
            return { headers: headers };
        };

        self.startUrl = null;

    }]);
})();