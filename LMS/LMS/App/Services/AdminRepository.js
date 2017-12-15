(function () {
    var app = angular.module("AdminApp")
        .factory("AdminRepository", ["BootstrappedData", "Helpers", "$q", function (BootstrappedData, Helpers, $q) {
            "use strict";

            var initUsers = function () {
                return BootstrappedData.data.Users;
            };
            var updateUsers = function () {
                var deferred = $q.defer();
                Helpers.postData("/api/admin/getusers")
                    .then(deferred.resolve, deferred.reject);
                return deferred.promise;
            };
            var createUser = function (user) {
                var deferred = $q.defer();
                Helpers.postData("/api/admin/createuser", user)
                    .then(deferred.resolve, deferred.reject);
                return deferred.promise;
            };
            var editUser = function (user) {
                var deferred = $q.defer();
                Helpers.postData("/api/admin/edituser", user)
                    .then(deferred.resolve, deferred.reject);
                return deferred.promise;
            };
            var deleteUser = function (user) {
                var deferred = $q.defer();
                Helpers.postData("/api/admin/deleteUser", user)
                    .then(deferred.resolve, deferred.reject);
                return deferred.promise;
            };

            var initRoles = function () {
                return BootstrappedData.data.Roles;
            };
            var updateRoles = function () {
                return Helpers.postData("/api/admin/getroles")
                    .then(function (response) {
                        return response.data;
                    });
            };
            var createRole = function (role) {
                var deferred = $q.defer();
                Helpers.postData("/api/admin/createrole", role)
                    .then(deferred.resolve, deferred.reject);
                return deferred.promise;
            };
            var editRole = function (role) {
                alert("Not implemented! Role: " + role.Name);
                //var deferred = $q.defer();
                //Helpers.postData("/api/admin/editrole", role)
                //    .then(deferred.resolve, deferred.reject);
                //return deferred.promise;
            };
            var deleteRole = function (role) {
                var deferred = $q.defer();
                Helpers.postData("/api/admin/deleterole", role)
                    .then(deferred.resolve, deferred.reject);
                return deferred.promise;
            };


            return {
                initUsers: initUsers,
                initRoles: initRoles,
                updateUsers: updateUsers,
                updateRoles: updateRoles,
                createUser: createUser,
                createRole: createRole,
                editUser: editUser,
                editRole: editRole,
                deleteUser: deleteUser,
                deleteRole: deleteRole
            };
        }]);

})();