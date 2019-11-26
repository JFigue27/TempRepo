'use strict';

/**
 * @ngdoc service
 * @name Training.baseControllers
 * @description
 * # baseControllers
 * Service in the Training.
 */
angular.module('Training').factory('formController', function($log, $routeParams, validatorService, appConfig, $timeout, $rootScope, $q, $activityIndicator) {

    var log = $log;

    return function(oMainConfig) {

        var formCtrl = this;

        //INIT CONFIG/////////////////////////////////////

        var scope = oMainConfig.scope;

        var _baseService = oMainConfig.baseService;
        if (!oMainConfig.entityName || oMainConfig.entityName == '') {
            oMainConfig.entityName = '';
        }

        //After Load callback
        var _afterLoadCallBack = oMainConfig.afterLoad;
        if (!_afterLoadCallBack || typeof _afterLoadCallBack != 'function') {
            _afterLoadCallBack = function() {};
        }

        //After create entity callback
        var _afterCreateCallBack = oMainConfig.afterCreate;
        if (!_afterCreateCallBack || typeof _afterCreateCallBack != 'function') {
            _afterCreateCallBack = function() {};
        }

        //END CONFIG/////////////////////////////////////


        //scope---------------------------------------------
        //let's use normal variables (without underscore) so they can be
        //accessed in view normally
        scope.isLoading = true;
        scope.remove = function(oEntity) {
            var deferred = $q.defer();
            alertify.confirm(
                '¿Está seguro que quiere eliminar: ' + oMainConfig.entityName + '?',
                function() {
                    scope.$apply(function() {
                        _baseService.remove(oEntity, scope.baseList).then(function(data) {
                            deferred.resolve();
                        }, function() {
                            deferred.reject();
                        });
                    });
                },
                function() {
                    deferred.reject();
                });
            return deferred.promise;
        };
        scope.take = function(objToTake, toUser) {
            _baseService.take(objToTake, toUser).then(function(data) {
                objToTake.assignedTo = toUser.Value;
                objToTake.AssignationMade = false;
            });
        };

        scope.create = function() {
            $activityIndicator.startAnimating();
            _baseService.createEntity().then(function(oNewEntity) {
                scope.baseEntity = angular.copy(oNewEntity);
                _afterCreateCallBack(scope.baseEntity);
                $activityIndicator.stopAnimating();
            });
        };

        var _saveChildren = function() {
            var arrPromiseConstructors = [];
            angular.forEach(_baseService._childrenResources, function(childCtrl) {
                var promiseConstructor = function() {
                    return childCtrl.save(childCtrl);
                }
                arrPromiseConstructors.push(angular.copy(promiseConstructor));
            });

            return $q.serial(arrPromiseConstructors);
        };

        //Updating items:*******************************
        formCtrl.save = function() {
            var deferred = $q.defer();
            $activityIndicator.startAnimating();

            _saveChildren().then(function() {
                if (scope.baseEntity.editMode) {
                    _baseService.save(scope.baseEntity).then(function(data) {
                        $activityIndicator.stopAnimating();
                        deferred.resolve();
                    }, function() {
                        deferred.reject();
                    });
                } else {
                    deferred.resolve();
                }
            });

            return deferred.promise;
        };
        scope.on_input_change = function(oItem) {
            oItem.editMode = true;
        };
        scope.undo = function(oItem) {
            var originalItem = _baseService.getById(oItem.id);
            angular.copy(originalItem, oItem);
        };
        //end scope----------------------------------------

        var _afterLoad = function() {
            _afterLoadCallBack(scope.baseEntity);
            $activityIndicator.stopAnimating();
        };

        formCtrl.load = function(oEntityOrID) {
            $activityIndicator.startAnimating();
            alertify.closeAll();

            _baseService.loadDependencies().then(function(data) {
                _fillCatalogs();
                _refreshForm(oEntityOrID);
            });
        };

        var _fillCatalogs = function() {
            //for filters:

            for (var catalog in _baseService.catalogs) {
                if (_baseService.catalogs.hasOwnProperty(catalog)) {

                    var theCatalog = 'the' + capitalizeFirstLetter(catalog);
                    if (theCatalog.slice(-1) != 's') {
                        theCatalog += 's';
                    }
                    scope[theCatalog] = _baseService.catalogs[catalog].getAll();
                }
            }

        };

        function capitalizeFirstLetter(sWord) {
            var result = sWord.charAt(0).toUpperCase() + sWord.slice(1).toLowerCase();
            return result;
        }

        var _makeQueryParameters = function() {
            var result = '?';

            for (var prop in scope.filterOptions) {
                if (scope.filterOptions.hasOwnProperty(prop)) {
                    result += prop + '=' + scope.filterOptions[prop] + '&';
                }
            }

            return result;
        };

        var _refreshForm = function(oEntityOrID) {
            $activityIndicator.startAnimating();
            scope.isLoading = true;

            //Create
            if (!oEntityOrID) {
                _baseService.createEntity().then(function(oNewEntity) {
                    scope.baseEntity = angular.copy(oNewEntity);
                    _afterCreateCallBack(oNewEntity);
                    $activityIndicator.stopAnimating();
                });
            }
            //Update by ID
            else if (!isNaN(oEntityOrID) && oEntityOrID > 0) {
                scope.openingMode = 'id';
                _baseService.loadEntity(oEntityOrID).then(function(data) {
                    var theSelectedEntity = data;
                    if (!theSelectedEntity) {
                        alertify.alert('¡El registro no existe!').set('modal', true).set('closable', false);
                        scope.openingMode = 'error';
                        return;
                    }
                    scope.baseEntity = angular.copy(theSelectedEntity);
                    _afterLoad();
                    scope.isLoading = false;
                });
            } else if (oEntityOrID instanceof Object || typeof(oEntityOrID) == 'object') {
                scope.baseEntity = angular.copy(oEntityOrID);
                _afterLoad();
                scope.isLoading = false;
            }
        };

        _baseService._childrenResources = [];
        formCtrl.addChildrenCtrl = function(childrenCtrl, id) {
            var oFound = _baseService._childrenResources.find(function(c) {
                return c.id == id;
            });
            if (!oFound) {
                childrenCtrl.id = id;
                _baseService._childrenResources.push(childrenCtrl);
            }
        };

        // // Public baseController API:////////////////////////////////////////////////////////////
        // var oAPI = {
        //     // load: _load,
        //     // addChildrenCtrl: _addChildrenCtrl,
        //     // childrenCtrls: _childrenResources
        //     // unselectAll: _unselectAll
        // };
        // return oAPI;
    };

});
