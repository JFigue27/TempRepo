'use strict';

/**
 * @ngdoc function
 * @name Main.controller:ReportsController
 * @description
 * # ReportsController
 * Controller of the Main
 */
angular.module('Training').controller('ReportsController', function($scope, appConfig) {
    $scope.baseURL = appConfig.API_URL;
});