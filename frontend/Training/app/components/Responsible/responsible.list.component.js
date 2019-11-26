'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:ResponsibleList
 * @description
 * # ResponsibleList
 */
angular.module('Training').directive('responsibleList', function($mdDialog) {
    return {
        templateUrl: 'components/Responsible/responsible.list.html',
        restrict: 'E',
        require: '^responsible',
        link: function(scope, element, attrs, responsibleCtrl) {
            scope.$on('ok-modal-responsible', function() {
                responsibleCtrl.changeUser(scope.currentSelectedId).then(function() {
                    $mdDialog.hide();
                });
            });
        },
        controller: function($scope, userService, listController, $rootScope) {

            var listCtrl = new listController({
                scope: $scope,
                entityName: 'User',
                baseService: userService,
                paginate: false,
                afterCreate: function(oInstance) {

                },
                afterLoad: function() {

                },
                onOpenItem: function(oEntity) {

                },
                filters: []
            });

            $scope.$on('load_responsible_list', function(scope, currentOne) {
                $scope.currentSelectedId = currentOne;
                refresh();
            });

            function refresh() {
                listCtrl.load();
            }

            $scope.selectItem = function(oItem) {
                $scope.currentSelectedId = oItem.id;
            };

        }
    };
});
