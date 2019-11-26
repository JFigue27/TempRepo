'use strict';

/**
 * @ngdoc function
 * @name Training.controller:UserListController
 * @description
 * # UserListController
 * Controller of the Training
 */
angular.module('Training').controller('UserListController', function($scope, listController, UserService) {

    var ctrl = new listController({
        scope: $scope,
        entityName: 'User',
        baseService: UserService,
        afterCreate: function(oInstance) {

        },
        afterLoad: function() {

        },
        onOpenItem: function(oItem) {

        },
        filters: {
            
        }
    });

    ctrl.load();

});
