'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:Training
 * @description
 * # Training
 */
angular.module('Training').directive('trainingList', function() {
    return {
        templateUrl: 'components/Training/training.list.html',
        restrict: 'E',
        controller: function($scope, listController, $mdDialog, $rootScope, TrainingService) {

            var listCtrl = new listController({
                scope: $scope,
                entityName: 'Training',
                baseService: TrainingService,
                afterCreate: function(oInstance, oEvent) {
                    $mdDialog.show({
                        title: 'Training',
                        contentElement: '#modal-Training',
                        parent: angular.element(document.body),
                        clickOutsideToClose: true,
                        multiple: true,
                        fullscreen: true,
                        targetEvent: oEvent,
                        onRemoving: function(element, removePromise) {
                            refresh();
                        }
                    });

                    $rootScope.$broadcast('load-modal-Training', oInstance);
                },
                afterLoad: function() {

                },
                onOpenItem: function(oEntity, oEvent) {
                    $mdDialog.show({
                        title: 'Training',
                        contentElement: '#modal-Training',
                        parent: angular.element(document.body),
                        clickOutsideToClose: false,
                        multiple: true,
                        fullscreen: true,
                        targetEvent: oEvent,
                        onRemoving: function(element, removePromise) {
                            refresh();
                        }
                    });

                    $rootScope.$broadcast('load-modal-Training', oEntity);
                },
                filters: []
            });

            $scope.$on('load_Training', function(scope) {
                refresh();
            });

            function refresh() {
                listCtrl.load('QuickTraining=false');
            }

            refresh();

        }
    };
});
