'use strict';

/**
 * @ngdoc directive
 * @name Training.directive:EmployeeCard
 * @description
 * # EmployeeCard
 */
angular.module('Training').directive('employeeCardForm', function() {
    return {
        templateUrl: 'components/EmployeeCard/employee.card.form.html',
        restrict: 'E',
        controller: function($scope, formController, $mdDialog, EmployeeCardService) {

            var ctrl = this;

            formController.call(this, {
                scope: $scope,
                entityName: 'Employee',
                baseService: EmployeeCardService,
                afterCreate: function(oEntity) {

                },
                afterLoad: function(oEntity) {

                }
            });

            $scope.$on('load-modal-EmployeeCard', function(scope, oEmployeeCard) {
                refresh(oEmployeeCard);
            });

            $scope.$on('ok-modal-EmployeeCard', function() {


                $scope.baseEntity.api_avatar.uploadAvatar().then(function() {

                    $scope.baseEntity.editMode = true;
                    return ctrl.save().then(function() {
                        $mdDialog.hide();
                        alertify.success('Saved Successfully.');
                    });

                });

            });

            function refresh(oEmployeeCard) {
                ctrl.load(oEmployeeCard);
            }

            $scope.onAvatarChange = function(oEntity) {

                var avatarAPI = $scope.baseEntity.api_avatar;

                $scope.baseEntity.api_avatar.uploadAvatar().then(function() {

                    $scope.baseEntity.editMode = true;
                    return ctrl.save().then(function() {
                        alertify.success('Image Saved Successfully.');
                        $scope.baseEntity.api_avatar = avatarAPI;
                    });

                });
            };

            $scope.$on('print-modal-EmployeeCard', function() {
                window.open('reports/training-profile.html');
            });

        }
    };
});
