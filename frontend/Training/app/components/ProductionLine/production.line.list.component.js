'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:ProductionLine
 * @description
 * # ProductionLine
 */
angular.module('Training').directive('productionLineList', function() {
    return {
        templateUrl: 'components/ProductionLine/production.line.list.html',
        restrict: 'E',
        controller: function($scope, listController, $mdDialog, $rootScope, ProductionLineService) {

            var listCtrl = new listController({
                scope: $scope,
                entityName: 'ProductionLine',
                baseService: ProductionLineService,
                afterCreate: function(oInstance, oEvent) {
                    $mdDialog.show({
                        title: 'ProductionLine',
                        contentElement: '#modal-ProductionLine',
                        parent: angular.element(document.body),
                        clickOutsideToClose: true,
                        multiple: true,
                        fullscreen: true,
                        targetEvent: oEvent,
                        onRemoving: function(element, removePromise) {
                            listCtrl.load();
                        }
                    });

                    $rootScope.$broadcast('load-modal-ProductionLine', oInstance);
                },
                afterLoad: function() {

                },
                onOpenItem: function(oEntity, oEvent) {
                    $mdDialog.show({
                        title: 'ProductionLine',
                        contentElement: '#modal-ProductionLine',
                        parent: angular.element(document.body),
                        clickOutsideToClose: true,
                        multiple: true,
                        fullscreen: true,
                        targetEvent: oEvent,
                        onRemoving: function(element, removePromise) {
                            listCtrl.load();
                        }
                    });

                    $rootScope.$broadcast('load-modal-ProductionLine', oEntity);
                },
                filters: []
            });

            $scope.$on('load_ProductionLine', function(scope) {
                listCtrl.load();
            });

            listCtrl.load();
        }
    };
});
