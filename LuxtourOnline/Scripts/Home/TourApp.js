﻿(function () {
    'use strict';

    var app = angular.module('TourApp', ['ngRoute', 'ngAnimate', 'duScroll']);

    app.controller('TourCtrl', ['$scope', '$http', '$window', '$document', '$location', function ($scope, $http, $window, $document, $location) {
        $scope.Data = null;

        $scope.PerPage = 10;
        $scope.Page = 1;
        $scope.pixelsScrolled = 0;

        $scope.header = angular.element('#cover-image-tours');
        $scope.grid = angular.element('#up-block');
        $scope.content = angular.element('#content');

        $scope.coverMargin = 0;

        $scope.Active = null;
        $scope.Disactive = [];

        $scope.SaveOffset = 0;


        $scope.SetActive = function (tour)
        {
            if (tour != null)
            {
                $scope.Active = tour;
                $scope.Disactive = [];

                for (var i = 0; i < $scope.Data.tours.length; i++)
                {
                    var t = $scope.Data.tours[i];

                    if ($scope.Active != t)
                    {
                        $scope.Disactive.push(t);
                    }
                }

                var e = angular.element('#cover-image-tours');
                var offset = -424 + $scope.coverMargin;
                var duration = 1000;

                $scope.SaveOffset = $scope.coverMargin;

                $scope.content.scrollToElement(e, offset, duration);
            }
            else {
                $scope.Active = null;

                var e = angular.element('#cover-image-tours');
                var offset = -$scope.SaveOffset;
                var duration = 1000;

                $scope.content.scrollToElement(e, offset, duration);
            }

        }

        $scope.LoadData = function () {

            $http({
                method: 'GET',
                url: '/Home/GetTours',
                params: {
                    page: $scope.Page,
                    toursPerPage: $scope.PerPage,
                },
            })
            .success(function (response) {
                if (response != null)
                {
                    console.log(response);

                    $scope.Data = response;
                }
            });
        };

        $scope.Order = function(id)
        {
            $window.location = '/Home/Order?id='+id;
        }

        angular.element('#content').on('scroll', function () {
            
            var s = $scope.content.scrollTop();

            if (s < 600)
            {
                $scope.coverMargin = s / 2.5;
            }

            $scope.$apply();
        });
        

    }]);

})();