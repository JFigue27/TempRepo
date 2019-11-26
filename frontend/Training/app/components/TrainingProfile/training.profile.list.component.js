'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:TrainingProfile
 * @description
 * # TrainingProfile
 */
angular.module('Training').directive('trainingProfileList', function() {
    return {
        templateUrl: 'components/TrainingProfile/training.profile.list.html',
        restrict: 'E',
        controller: function($scope, listController, $mdDialog, $rootScope, TrainingProfileService) {

            var listCtrl = new listController({
                scope: $scope,
                entityName: 'TrainingProfile',
                baseService: TrainingProfileService,
                afterCreate: function(oInstance, oEvent) {
                    $mdDialog.show({
                        title: 'TrainingProfile',
                        contentElement: '#modal-TrainingProfile',
                        parent: angular.element(document.body),
                        clickOutsideToClose: true,
                        multiple: true,
                        fullscreen: true,
                        targetEvent: oEvent,
                        onRemoving: function(element, removePromise) {
                            listCtrl.load();
                        }
                    });

                    $rootScope.$broadcast('load-modal-TrainingProfile', oInstance);
                },
                afterLoad: function() {

                },
                onOpenItem: function(oEntity, oEvent) {
                    $mdDialog.show({
                        title: 'TrainingProfile',
                        contentElement: '#modal-TrainingProfile',
                        parent: angular.element(document.body),
                        clickOutsideToClose: true,
                        multiple: true,
                        fullscreen: true,
                        targetEvent: oEvent,
                        onRemoving: function(element, removePromise) {
                            listCtrl.load();
                        }
                    });

                    $rootScope.$broadcast('load-modal-TrainingProfile', oEntity);
                },
                filters: []
            });

            $scope.$on('load_TrainingProfile', function(scope) {
                listCtrl.load();
            });

            listCtrl.load();
        }
    };
});
