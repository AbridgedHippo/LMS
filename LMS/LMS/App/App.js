(function () {
    // main module
    var app = angular.module("LMSApp", ["AccApp", "AdminApp"]);
    var serviceApp = angular.module("ServiceApp", ["ui.bootstrap"]);
    var accApp = angular.module("AccApp", ["ServiceApp"]);
    var adminApp = angular.module("AdminApp", ["ServiceApp", "ngRoute"])
        .config(function ($routeProvider, $locationProvider) {
            $routeProvider
                .when("/Admin", {
                    templateUrl: "/App/Partials/Admin/Index.cshtml"
                })
                .when("/Admin/Users", {
                    templateUrl: "/App/Partials/Admin/Users.cshtml"
                })
                .when("/Admin/Roles", {
                    templateUrl: "/App/Partials/Admin/Roles.cshtml"
                });
            $locationProvider.html5Mode({
                enabled: true,
                requireBase: false,
                rewriteLinks: "admin-link"
            });
        });

})();