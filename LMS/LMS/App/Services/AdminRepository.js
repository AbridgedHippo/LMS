(function () {
    var app = angular.module("AdminApp")
        .factory("AdminRepository", ["BootstrappedData", "Helpers", "$q", function (BootstrappedData, Helpers, $q) {
            "use strict";

            // Users
            var initUsers = function () {
                return BootstrappedData.data.Users;
            };
            var getUser = function (user) {
                var deferred = $q.defer();
                Helpers.postData("/api/admin/getuser", user)
                    .then(deferred.resolve, deferred.reject);
                return deferred.promise;
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
            var setPassword = function (data) {
                console.log(data);
                var deferred = $q.defer();
                Helpers.postData("/api/admin/setpassword", data)
                    .then(deferred.resolve, deferred.reject);
                return deferred.promise;
            };

            // Roles
            var initRoles = function () {
                return BootstrappedData.data.Roles;
            };
            var getRole = function (role) {
                var deferred = $q.defer();
                Helpers.postData("/api/admin/getrole", role)
                    .then(deferred.resolve, deferred.reject);
                return deferred.promise;
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
                var deferred = $q.defer();
                Helpers.postData("/api/admin/editrole", role)
                    .then(deferred.resolve, deferred.reject);
                return deferred.promise;
            };
            var deleteRole = function (role) {
                var deferred = $q.defer();
                Helpers.postData("/api/admin/deleterole", role)
                    .then(deferred.resolve, deferred.reject);
                return deferred.promise;
            };

            // Courses
            var initCourses = function () {
                return BootstrappedData.data.Courses;
            };
            var getCourse = function (course) {
                var deferred = $q.defer();
                Helpers.postData("/api/admin/getcourse", course)
                    .then(deferred.resolve, deferred.reject);
                return deferred.promise;
            };
            var updateCourses = function () {
                var deferred = $q.defer();
                Helpers.postData("/api/admin/getcourses")
                    .then(deferred.resolve, deferred.reject);
                return deferred.promise;
            };
            var createCourse = function (course) {
                var deferred = $q.defer();
                Helpers.postData("/api/admin/createcourse", course)
                    .then(deferred.resolve, deferred.reject);
                return deferred.promise;
            };
            var editCourse = function (course) {
                var deferred = $q.defer();
                Helpers.postData("/api/admin/editcourse", course)
                    .then(deferred.resolve, deferred.reject);
                return deferred.promise;
            };
            var deleteCourse = function (course) {
                var deferred = $q.defer();
                Helpers.postData("/api/admin/deletecourse", course)
                    .then(deferred.resolve, deferred.reject);
                return deferred.promise;
            };

            // Users & Courses
            var addUserToCourse = function (user, course) {
                var data = {
                    User: user,
                    Course: course
                };
                var deferred = $q.defer();
                Helpers.postData("/api/admin/addusertocourse", data)
                    .then(deferred.resolve, deferred.reject);
                return deferred.promise;
            };
            var removeUserFromCourse = function (user, course) {
                var data = {
                    User: user,
                    Course: course
                };
                var deferred = $q.defer();
                Helpers.postData("/api/admin/removeuserfromcourse", data)
                    .then(deferred.resolve, deferred.reject);
                return deferred.promise;
            };
            var getStudentsForCourse = function (course) {
                var deferred = $q.defer();
                Helpers.postData("/api/admin/getstudentsforcourse", course)
                    .then(deferred.resolve, deferred.reject);
                return deferred.promise;

            };
            var getTeachersForCourse = function (course) {
                var deferred = $q.defer();
                Helpers.postData("/api/admin/getteachersforcourse", course)
                    .then(deferred.resolve, deferred.reject);
                return deferred.promise;

            };

            // Course Assignments
            var getAssignmentsForCourse = function (course) {
                var deferred = $q.defer();
                Helpers.postData("/api/admin/getassignmentsforcourse", course)
                    .then(deferred.resolve, deferred.reject);
                return deferred.promise;
            };
            var createAssignmentForCourse = function (assignment) {
                var deferred = $q.defer();
                Helpers.postData("/api/admin/createassignmentforcourse", assignment)
                    .then(deferred.resolve, deferred.reject);
                return deferred.promise;
            };
            var deleteAssignmentFromCourse = function (assignment) {
                var deferred = $q.defer();
                Helpers.postData("/api/admin/deleteassignmentfromcourse", assignment)
                    .then(deferred.resolve, deferred.reject);
                return deferred.promise;
            };
            var editAssignmentForCourse = function (assignment) {
                var deferred = $q.defer();
                Helpers.postData("/api/admin/editAssignmentForCourse", assignment)
                    .then(deferred.resolve, deferred.reject);
                return deferred.promise;
            };
            var getAssignment = function (assignment) {
                var deferred = $q.defer();
                Helpers.postData("/api/admin/getassignment", assignment)
                    .then(deferred.resolve, deferred.reject);
                return deferred.promise;
            };

            return {
                initUsers: initUsers,
                initRoles: initRoles,
                initCourses: initCourses,
                updateUsers: updateUsers,
                updateRoles: updateRoles,
                updateCourses: updateCourses,
                createUser: createUser,
                createRole: createRole,
                createCourse: createCourse,
                editUser: editUser,
                editRole: editRole,
                editCourse: editCourse,
                deleteUser: deleteUser,
                deleteRole: deleteRole,
                deleteCourse: deleteCourse,
                getUser: getUser,
                getRole: getRole,
                getCourse: getCourse,
                addUserToCourse: addUserToCourse,
                removeUserFromCourse: removeUserFromCourse,
                getStudentsForCourse: getStudentsForCourse,
                getTeachersForCourse: getTeachersForCourse,
                getAssignmentsForCourse: getAssignmentsForCourse,
                createAssignmentForCourse: createAssignmentForCourse,
                deleteAssignmentFromCourse: deleteAssignmentFromCourse,
                editAssignmentForCourse: editAssignmentForCourse,
                getAssignment: getAssignment,
                setPassword: setPassword
            };
        }]);

})();