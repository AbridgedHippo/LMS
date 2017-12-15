(function () {
    var app = angular.module("ServiceApp")
        .factory("Helpers", ["$http", "$window", "$q", function ($http, $window, $q) {
        "use strict";

        // Session storage
        var setSessionId = function (key, value) {
            $window.sessionStorage.setItem(key, value);
        };
        var getSessionId = function (key) {
            return $window.sessionStorage.getItem(key);
        };
        var clearStorage = function () {
            $window.sessionStorage.clear();
            $window.localStorage.clear();
        };
        var setTokenKey = function (value) {
            $window.sessionStorage.setItem("tokenKey", value);
        };
        var getTokenKey = function () {
            var token = $window.sessionStorage.getItem("tokenKey");
            return token;
        };

        // http get & post
        var getData = function (url) {
            var config = getTokenHeaders();
            var deferred = $q.defer();
            $http.get(url, config)
                .then(deferred.resolve, deferred.reject);
            return deferred.promise;
        };
        var postData = function (url, data) {
            var config = getTokenHeaders();
            var deferred = $q.defer();
            $http.post(url, data, config)
                .then(deferred.resolve, deferred.reject);
            return deferred.promise;
        };

        var goTo = function (targetUrl) {
            document.location.href = targetUrl;
        };

        // Helper functions to the helper functions :)
        var getTokenHeaders = function () {
            var token = getTokenKey();
            var headers = {};
            if (token) {
                headers.Authorization = "Bearer " + token;
                return { headers: headers };
            }
            return null;
        };

        return {
            setSessionId: setSessionId,
            getSessionId: getSessionId,
            clearStorage: clearStorage,
            setTokenKey: setTokenKey,
            getTokenKey: getTokenKey,
            getData: getData,
            postData: postData,
            goTo: goTo
        };
    }]);

})();