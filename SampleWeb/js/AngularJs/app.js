

var CosmolexApp = angular.module("CosmolexApp", []).factory("CosmolexService", function() {
return {
    
    Author: { Name: "Umais Siddiqui" },
    PostData: function (callSuccess, errorCallback, resource, params) {
       var request = $.ajax({
            beforeSend: function (xhrObj) {
                xhrObj.setRequestHeader("Content-Type", "application/json");
                xhrObj.setRequestHeader("Accept", "application/json");
            },
      
            url: location.protocol + "//" + location.host + "/api/" + resource,
            type: "post",
            data: JSON.stringify(params),
            success: function (response) { callSuccess(response); },
            error: function (result) { errorCallback(result); }
        });
    }
}
}).controller("CaseController",function($scope,$filter, CosmolexService) {

    $scope.Author = CosmolexService.Author.Name;
    $scope.resource = "CaseEntity";
    $scope.showLoadingGif = false;
    $scope.noCases = false;
    $scope.showCaseList = false;
    $scope.BodyParam = {
        ConnectionDetails: {
            servername: "",
            databasename: "",
            username: "",
            password: ""
        },
        Model: {
            Id: "",
            Name: "",
            StartDate: "",
            CaseType: "",
            CaseDescription: "",
            Active: ""
        }

    };

    $scope.GetCases = function() {
        $scope.showCaseList = false;
        $scope.CaseList = [];
        if ($scope.validate()) {
            $scope.showLoadingGif = true;
            CosmolexService.PostData($scope.GetSuccess, $scope.CallError, $scope.resource + "/Get", $scope.BodyParam);
        } else {
            alert("make sure you have provided the connection details.");
        }
    };
    $scope.AddNew = function () {
        $scope.showLoadingGif = true;
        $scope.showCaseList = false;
        CosmolexService.PostData($scope.GetSuccess, $scope.CallError, $scope.resource + "/Add", $scope.BodyParam);
        $('#closeModal').click();
    };

    $scope.UpdateCase = function (ind) {
        $scope.showLoadingGif = true;
        $scope.showCaseList = false;
        $scope.BodyParam.Model = $scope.CaseList[ind];
        CosmolexService.PostData($scope.GetSuccess, $scope.CallError, $scope.resource + "/Update", $scope.BodyParam);
        
        
    };

    $scope.DeleteCase = function (ind) {
        $scope.showLoadingGif = true;
        $scope.showCaseList = false;
        $scope.BodyParam.Model = $scope.CaseList[ind];
        CosmolexService.PostData($scope.GetSuccess, $scope.CallError, $scope.resource + "/Delete", $scope.BodyParam);
    }
    $scope.GetSuccess = function (response) {
        $scope.showLoadingGif = false;
        
        $scope.CaseList = response;
        if ($scope.CaseList.length > 0 && $scope.CaseList[0].ErrorMessage !== null) {
            $scope.resetModel();
            $('#err').html($scope.CaseList[0].ErrorMessage);
            $('#home').click();
            return;
        }
        for (var i = 0; i < $scope.CaseList.length; i++) {
            $scope.CaseList[i].StartDate = $filter('date')($scope.CaseList[i].StartDate, 'shortDate');
        }
        if ($scope.CaseList.length === 0)
            $scope.noCases = true;
        else {
            $scope.noCases = false;
        }
        $scope.resetModel();
        $scope.showCaseList = true;
        $scope.$apply();
       
        $('.dp').datepicker();
        $('#work').click();
    };

   
    $scope.CallError = function (err) {
        $scope.resetModel();
        alert(err.statusMessage);
    };
    $scope.resetModel = function() {
        $scope.BodyParam.Model = {
            Id: "",
            Name: "",
            StartDate: "",
            CaseType: "",
            CaseDescription: "",
            Active: ""
        };
    };
    $scope.validate=function() {
        
        if ($scope.BodyParam.ConnectionDetails.servername !== "" && $scope.BodyParam.ConnectionDetails.databasename !== "" && $scope.BodyParam.ConnectionDetails.username !== "" && $scope.BodyParam.ConnectionDetails.password !== "") {

            return true;
        } else {
            return false;
        }
    }

    $scope.showHidePanel = function (a) {
       var $this = $('#minus' + a);
        var $collapse = $this.closest('.collapse-group').find('.collapse');

        $collapse.collapse('toggle');
        var $cl = $this.attr("class");
        var $newCl = "";
        var $ind = -1;

        $ind = $cl.indexOf("minus");
        if ($ind > 0) {
            $newCl = $cl.replace("minus", "plus");
        }
        else {
            $newCl = $cl.replace("plus", "minus");
        }

        $this.attr("class", $newCl);
    };
});

    
