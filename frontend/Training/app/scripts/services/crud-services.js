'use strict';

/**
 * @ngdoc service
 * @name Training.userService
 * @description
 * # userService
 * Service in the Training.
 */

angular.module('Training').service('utilsService', function($filter) {

    var service = {};

    service.toJavascriptDate = function(sISO_8601_Date) {
        return sISO_8601_Date ? moment(sISO_8601_Date, moment.ISO_8601).toDate() : null;
    };

    service.toServerDate = function(oDate) {
        var momentDate = moment(oDate);
        if (momentDate.isValid()) {
            momentDate.local();
            return momentDate.format();
        }
        return null;
    };

    service.toImageBase64 = function(imageString) {
        var result = '';

        if (imageString && imageString.length > 0) {
            result = 'data:image/bmp;base64,' + imageString;
        }
        return result;
    };

    return service;

}).service('userService', function(crudFactory) {
    var crudInstance = new crudFactory({
        //Entity Name = WebService/API to call:
        entityName: 'User',

        catalogs: [],

        adapter: function(theEntity) {
            return theEntity;
        },

        adapterIn: function(theEntity) {},

        adapterOut: function(theEntity, self) {
            theEntity.Identicon = '';
            theEntity.Identicon64 = '';
        },

        dependencies: [

        ]
    });

    crudInstance.getByUserName = function(sUserName) {
        var _arrAllRecords = crudInstance.getAll();
        for (var i = 0; i < _arrAllRecords.length; i++) {
            if (_arrAllRecords[i].UserName.toLowerCase() == sUserName.toLowerCase()) {
                return _arrAllRecords[i];
            }
        }
        return {
            id: -1,
            Value: ''
        };
    };

    crudInstance.getUsersInRoles = function(arrRoles) {
        var _arrAllRecords = crudInstance.getAll();
        var result = [];
        for (var i = 0; i < _arrAllRecords.length; i++) {
            if (arrRoles.indexOf(_arrAllRecords[i].Role) > -1) {
                result.push(_arrAllRecords[i]);
            }
        }
        result.push(_arrAllRecords[0]);
        return result;
    };


    crudInstance.sendTestEmail = function(oUser) {
        return crudInstance.customPost('SendTestEmail', oUser);
    };

    return crudInstance;
}).service('trackService', function(crudFactory) {
    var crudInstance = new crudFactory({
        //Entity Name = WebService/API to call:
        entityName: 'Track',

        catalogs: [],

        adapter: function(theEntity) {
            return theEntity;
        },

        adapterIn: function(theEntity) {},

        adapterOut: function(theEntity, self) {

        },

        dependencies: [

        ]
    });

    crudInstance.assignResponsible = function(iTrackKey, iUserKey) {
        return crudInstance.customPost('AssignResponsible/' + iTrackKey + '/' + iUserKey);
    };

    return crudInstance;
});
