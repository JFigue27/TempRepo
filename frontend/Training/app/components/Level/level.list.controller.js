'use strict';

/**
 * @ngdoc function
 * @name Training.controller:LevelListController
 * @description
 * # LevelListController
 * Controller of the Training
 */
angular.module('Training').controller('LevelListController', function($scope, listController, $mdDialog, $rootScope, LevelService) {

    var listCtrl = new listController({
        scope: $scope,
        entityName: 'Level',
        baseService: LevelService,
        afterCreate: function(oInstance, oEvent) {
            $mdDialog.show({
                title: 'Level',
                contentElement: '#modal-Level',
                parent: angular.element(document.body),
                clickOutsideToClose: true,
                multiple: true,
                fullscreen: true,
                targetEvent: oEvent,
                onRemoving: function(element, removePromise) {
                    listCtrl.load();
                }
            });

            $rootScope.$broadcast('load-modal-Level', oInstance);
        },
        afterLoad: function() {

        },
        onOpenItem: function(oEntity, oEvent) {
            $mdDialog.show({
                title: 'Level',
                contentElement: '#modal-Level',
                parent: angular.element(document.body),
                clickOutsideToClose: true,
                multiple: true,
                fullscreen: true,
                targetEvent: oEvent,
                onRemoving: function(element, removePromise) {
                    listCtrl.load();
                }
            });

            $rootScope.$broadcast('load-modal-Level', oEntity);
        },
        filters: []
    });

    $scope.$on('load_Level', function(scope) {
        listCtrl.load();
    });

    listCtrl.load();
});
