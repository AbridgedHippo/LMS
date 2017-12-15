(function () {
    // main module
    var app = angular.module("AccApp").controller("AccountController", ["Helpers", "$scope", "$uibModal", function (Helpers, $scope, $uibModal) {
        "use strict";
        var $ctrl = this;
        $ctrl.user = JSON.parse(Helpers.getSessionId("User"));
        $ctrl.login = {
            "UserName": "Admin",
            "Password": "Admin-1234"
        };
        $ctrl.passwords = {
            OldPassword: "",
            NewPassword: "",
            ConfirmPassword: ""
        };
        $ctrl.response = "";

        $ctrl.account = function () {
            $uibModal.open({
                templateUrl: "/App/Partials/Modals/AccountModal.cshtml",
                scope: $scope
            })
                .result.then(function () {
                    $ctrl.changePassword();
                }, function () {
                });
        };

        $ctrl.changePassword = function () {
            Helpers.postData("/api/account/changepassword", $ctrl.passwords)
                .then(function (response) {
                    $ctrl.response = response.data;
                    console.log($ctrl.response);
                }, function (error) {
                    $ctrl.response = error.data;
                    console.log($ctrl.response);
                });

        };

        $ctrl.requestLogin = function () {
            var data = "grant_type=password&username="
                + $ctrl.login.UserName
                + "&password="
                + $ctrl.login.Password;

            Helpers.postData("/Token", data)
                .then(function (response) {
                    var user = {
                        UserName: response.data.userName,
                        Email: "",
                        Id: ""
                    };
                    Helpers.setSessionId("User", JSON.stringify(user));
                    Helpers.setTokenKey(response.data.access_token);
                    Helpers.goTo("/");
                }, function (error) {
                    $ctrl.response = error.data;
                });
        };

        $ctrl.logOut = function () {
            Helpers.postData("/api/account/logout")
                .then(function (response) {
                    $ctrl.response = response.data;
                    Helpers.clearStorage();
                    Helpers.goTo("/");
                }, function (error) {
                    $ctrl.response = error.data;
                });
        };
    }]);
})();