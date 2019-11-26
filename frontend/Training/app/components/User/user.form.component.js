'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:UserForm
 * @description
 * # UserForm
 */
angular.module('Training').directive('userForm', function() {
    return {
        templateUrl: 'components/User/user.form.html',
        restrict: 'E',
        controller: function($scope, formController, userService, $mdDialog) {

            var ctrl = this;

            formController.call(this, {
                scope: $scope,
                entityName: 'User',
                baseService: userService,
                afterCreate: function(oEntity) {

                },
                afterLoad: function(oEntity) {

                }
            });

            $scope.$on('load-modal-user', function(scope, oUser) {
                refresh(oUser);
            });

            $scope.$on('ok-modal-user', function() {
                return ctrl.save().then(function() {
                    $mdDialog.hide();
                });
            });

            function refresh(oUser) {
                ctrl.load(oUser);
            }
        }
    };
});
