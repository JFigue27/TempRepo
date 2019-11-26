'use strict';

/**
 * @ngdoc function
 * @name Training.controller:LoginController
 * @description
 * # LoginController
 * Controller of the Training
 */
angular.module('Training').controller('LoginController', function($scope, $location, authService,  $activityIndicator ,appConfig) {

    $scope.appName = appConfig.APP_NAME;

    $activityIndicator.stopAnimating();
    alertify.closeAll();

    $scope.loginData = {
        userName: "",
        password: ""
    };

    $scope.ErrorMessage = null;

    $scope.login = function() {
        $scope.ErrorMessage = null;
        $activityIndicator.startAnimating();
        authService.login($scope.loginData).then(function(response) {
                $location.path('/');
                $activityIndicator.stopAnimating();
            },
            function(err) {
                $activityIndicator.stopAnimating();
                if (err == 'invalid_grant') {
                    err = 'Invalid username or password.';
                }
                $scope.ErrorMessage = err;
            });
    };
});
