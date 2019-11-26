'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:FormatDc3
 * @description
 * # FormatDc3
 */
angular.module('Training').directive('formatDc3List', function() {
    return {
        templateUrl: 'components/FormatDc3/format.dc.3.list.html',
        restrict: 'E',
        controller: function($scope, listController, $mdDialog, $rootScope, FormatDc3Service) {

            var listCtrl = new listController({
                scope: $scope,
                entityName: 'FormatDc3',
                baseService: FormatDc3Service,
                afterCreate: function(oInstance, oEvent) {
                    $mdDialog.show({
                        title: 'FormatDc3',
                        contentElement: '#modal-FormatDc3',
                        parent: angular.element(document.body),
                        clickOutsideToClose: true,
                        multiple: true,
                        fullscreen: true,
                        targetEvent: oEvent,
                        onRemoving: function(element, removePromise) {
                            listCtrl.load();
                        }
                    });

                    $rootScope.$broadcast('load-modal-FormatDc3', oInstance);
                },
                afterLoad: function() {

                },
                onOpenItem: function(oEntity, oEvent) {
                    $mdDialog.show({
                        title: 'FormatDc3',
                        contentElement: '#modal-FormatDc3',
                        parent: angular.element(document.body),
                        clickOutsideToClose: true,
                        multiple: true,
                        fullscreen: true,
                        targetEvent: oEvent,
                        onRemoving: function(element, removePromise) {
                            listCtrl.load();
                        }
                    });

                    $rootScope.$broadcast('load-modal-FormatDc3', oEntity);
                },
                filters: []
            });

            $scope.$on('load_FormatDc3', function(scope) {
                listCtrl.load();
            });

            listCtrl.load();
        }
    };
});
