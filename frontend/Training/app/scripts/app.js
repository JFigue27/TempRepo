'use strict';

/**
 * @ngdoc overview
 * @name Training
 * @description
 * # Training
 *
 * Main module of the application.
 */
angular.module('Training', [
    'ngAnimate',
    'ngRoute',
    'ngSanitize',
    'ngActivityIndicator',
    'LocalStorageModule',
    'angularUtils.directives.dirPagination',
    'angularFileUpload',
    'ngMaterial',
    'ngWig'
], function($httpProvider) {
    $httpProvider.defaults.headers.post['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';
    $httpProvider.defaults.headers.put['Content-Type'] = 'application/x-www-form-urlencoded;charset=utf-8';
}).config(function($routeProvider, appConfig, $httpProvider, localStorageServiceProvider, $mdAriaProvider, $mdThemingProvider, $activityIndicatorProvider, $qProvider) {

    $mdAriaProvider.disableWarnings();

    $mdThemingProvider.theme('default')
        .primaryPalette('grey')
        .accentPalette('grey');

    localStorageServiceProvider.setPrefix(appConfig.APP_NAME);

    $qProvider.errorOnUnhandledRejections(false);

    ///start:generated:routes<<<
    $routeProvider
        .when('/', {
            templateUrl: 'routes/Main/main-route.html',
            controller: 'MainController',
            controllerAs: 'route_Main'
        })
        .when('/login', {
            templateUrl: 'routes/Login/login-route.html',
            controller: 'LoginController',
            controllerAs: 'route_Login'
        })
        .when('/about', {
            templateUrl: 'routes/About/about-route.html',
            controller: 'AboutController',
            controllerAs: 'route_About'
        })
        .when('/profile', {
            templateUrl: 'routes/Profile/profile-route.html',
            controller: 'ProfileController',
            controllerAs: 'route_Profile'
        })
        .when('/users', {
            templateUrl: 'routes/Users/users-route.html',
            controller: 'UsersController',
            controllerAs: 'route_Users'
        })
        .when('/employees', {
            templateUrl: 'routes/Employees/employees-route.html',
            controller: 'EmployeesController',
            controllerAs: 'route_Employees'
        })
        .when('/areas', {
            templateUrl: 'routes/Areas/areas-route.html',
            controller: 'AreasController',
            controllerAs: 'route_Areas'
        })
        .when('/workstations', {
            templateUrl: 'routes/Workstations/workstations-route.html',
            controller: 'WorkstationsController',
            controllerAs: 'route_Workstations'
        })
        .when('/certifications', {
            templateUrl: 'routes/Certifications/certifications-route.html',
            controller: 'CertificationsController',
            controllerAs: 'route_Certifications'
        })
        .when('/shifts', {
            templateUrl: 'routes/Shifts/shifts-route.html',
            controller: 'ShiftsController',
            controllerAs: 'route_Shifts'
        })
        .when('/training-profile', {
            templateUrl: 'routes/TrainingProfile/training-profile-route.html',
            controller: 'TrainingProfileController',
            controllerAs: 'route_TrainingProfile'
        })
        .when('/job-positions', {
            templateUrl: 'routes/JobPositions/job-positions-route.html',
            controller: 'JobPositionsController',
            controllerAs: 'route_JobPositions'
        })
        .when('/skills', {
            templateUrl: 'routes/Skills/skills-route.html',
            controller: 'SkillsController',
            controllerAs: 'route_Skills'
        })
        .when('/reports', {
            templateUrl: 'routes/Reports/reports-route.html',
            controller: 'ReportsController',
            controllerAs: 'route_Reports'
        })
        .when('/matrix', {
            templateUrl: 'routes/Matrix/matrix-route.html',
            controller: 'MatrixController',
            controllerAs: 'route_Matrix'
        })
        .when('/trainings', {
            templateUrl: 'routes/Trainings/trainings-route.html',
            controller: 'TrainingsController',
            controllerAs: 'route_Trainings'
        })
        .when('/dc-3', {
            templateUrl: 'routes/Dc3/dc-3-route.html',
            controller: 'Dc3Controller',
            controllerAs: 'route_Dc3'
        })
        .when('/dc-3-report/:ids', {
            templateUrl: 'routes/Dc3/dc-3-report-route.html',
            controller: 'Dc3Controller',
            controllerAs: 'route_Dc3'
        })
        .otherwise({redirectTo:'/'});
    ///end:generated:routes<<<

    $activityIndicatorProvider.setActivityIndicatorStyle('CircledWhite');
    alertify.set('notifier', 'position', 'top-left');
    alertify.set('notifier', 'delay', 2);

    $httpProvider.interceptors.push('authInterceptorService');

}).run(function(authService, $rootScope, $location) {

    authService.fillAuthData();

    // register listener to watch route changes
    $rootScope.$on('$routeChangeSuccess', function(event, next, current) {
        alertify.closeAll();
        $('.modal').modal('hide');
        $('.modal-backdrop.fade.in').remove();


        var authentication = authService.authentication;
        if (!authentication || !authentication.isAuth) {
            if (next.templateUrl != 'components/login/login.html') {
                $location.path('/login');
            }
        } else {
            //Role Validations
            // if (authentication.role == 'Usuario' || authentication.role == '') {
            //     authService.logOut();
            //     setTimeout(function() {
            //         alertify.alert('Only Administrators have access to this application.').set('modal', true);
            //     }, 300);
            // }
        }

    });

    $rootScope.$on('$routeChangeSuccess', function() {
        $rootScope.activePath = $location.path();
    });


    $rootScope.logOut = function() {
        authService.logOut();
    };

});
