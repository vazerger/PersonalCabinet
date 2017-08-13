app.controller('FormController', function ($scope, $http, $timeout, $location, $modal) {

    var getResults = $location.search()["result"];
    var getDate = $location.search()["dt"];
    var getDate1 = $location.search()["dt1"];
  
   // loaders
    $scope.loaderResults = { loading: false };
    $scope.loaderTable = { loading: false };

    // RESULTS
    $scope.getResults = function () {

        $scope.loaderResults.loading = true; // preloader
        $scope.msg = ""; // remove msg

        $http.get('/api/Data/GetResults').then(function (resp) {

                $scope.results = resp.data;

                for (var i = 0; i < resp.data.length; i++) {
                    if (resp.data[i].selected == true) {
                        $scope.result = resp.data[i];
                    }
                }
        }, function (err) {
            console.error('ERR', err);
        })
        .finally(function () {
            $scope.loaderResults.loading = false;
        })
        .catch(function (err) {
            console.error('ERR', err);
        });
    };
    $scope.getResults();

    // Calendar
    $scope.today = function () {
        if (getDate) {
            getDate = getDate.replace(/^(\d+)\-(\d{2})\-(\d{4})/, '$3/$2/$1');
            $scope.dt = new Date(getDate);

            getDate1 = getDate.replace(/^(\d+)\-(\d{2})\-(\d{4})/, '$3/$2/$1');
            $scope.dt1 = new Date(getDate);
        }
        else {
            $http.get('/api/Data/GetTransactionsDates').then(function (resp) {

                getDate = resp.data.dateFrom;
                getDate = getDate.replace(/^(\d+)\-(\d{2})\-(\d{4})/, '$3/$2/$1');
                $scope.dt = new Date(getDate);

                getDate1 = resp.data.dateTo;
                getDate1 = getDate1.replace(/^(\d+)\-(\d{2})\-(\d{4})/, '$3/$2/$1');
                $scope.dt1 = new Date(getDate1);

            }, function (err) {
                console.error('ERR', err);
            })
             .finally(function () {
                 $scope.loaderResults.loading = false;
             })
             .catch(function (err) {
                 console.error('ERR', err);
             });

        }
    };
    $scope.today();

    $scope.openCalendar = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.opened = true;
    }

    $scope.dateOptions = {
        startingDay: 1
    };

    $scope.openCalendar1 = function ($event) {
        $event.preventDefault();
        $event.stopPropagation();
        $scope.opened1 = true;
    }

    $scope.dateOptions1 = {
        startingDay: 1
    };

    Date.prototype.format = function (mask, utc) {
        return dateFormat(this, mask, utc);
    };

    function FDate(d) {
        var curr_date = d.getDate();
        var curr_month = d.getMonth() + 1; //Months are zero based
        var curr_year = d.getFullYear();
        var modDate = curr_date + "-" + curr_month + "-" + curr_year;

        return modDate;
    }

    $scope.getTable = function () {

        $scope.loaderTable.loading = true;

        var page = 1;
        var size = 10;

        // begin
        setTimeout(function () {
                $http.get('/api/Data/GetTransactions/', {
                    params: {
                        search: $scope.orderid,
                        purchaseamt: $scope.purchaseamt,
                        datefrom: $scope.dt,
                        dateto: $scope.dt1,
                        result: $scope.result
                    },headers: {
                        'Cache-Control': 'no-cache'
                    }
                }).then(function (resp) {
                    $scope.tdatas = resp.data.transactions;
                    $scope.total_count = resp.data.total_count;
                    if (resp.data.total_count == 0) {
                        $scope.msg = "Ничего не найдено! Попробуйте изменить параметры поиска!";
                    }
                }, function (err) {
                    console.error('ERR', err);
                })
                .finally(function () {
                    $scope.loaderTable.loading = false;
                }).catch(function (err) {
                    console.error('ERR', err);
                });

        }, 100);
        // end

    }
    $scope.getTable();

    $scope.pagination = {
        current: 1
    };

    $scope.sort = function (keyname) {
        $scope.sortKey = keyname;
        $scope.reverse = !$scope.reverse;
    }

    // Search
    $scope.SearchOrderid = function (id) {

            $scope.orderid = id;
            $scope.getTable();

    }

    $scope.SearchPurchaseamt = function (id) {

            $scope.purchaseamt = id;
            $scope.getTable();

    }

    $scope.selectResult = function (result) {
        if (result == null) {
            $scope.result = '';
        }
        else {
            $scope.result = result.id;
        }

        $scope.getTable();

    }

    $scope.selDate = function (dt) {
       $scope.getTable();
    }

    $scope.selDate1 = function (dt1) {
        $scope.getTable();
    }

    $scope.viewDetail = function (id) {
        var modalInstance = $modal.open(
            {
                controller: 'DialogView',
                templateUrl: '/Home/ViewDetail',
                backdrop: true,
                backdropClick: true,
                dialogFade: false,
                resolve: {
                    callerData: function () { return { id: id }; }
                }
            })
        modalInstance.result.then(function () { $scope.loadData(); });
    }
});





app.controller('DialogView', function ($scope, $modalInstance, callerData, $http) {

    $scope.id = callerData.id;

    $http.get('/api/GetResults/', {
        params: {
            id: callerData.id,
        }, headers: {
            'Cache-Control': 'no-cache'
        }
    }).then(function (resp) {
        $scope.ORDERID = resp.data.ORDERID;
        $scope.PURCHASEAMT = resp.data.PURCHASEAMT;
        $scope.ACCOUNT = resp.data.ACCOUNT;
        $scope.RESULT = resp.data.RESULT;
        $scope.RESULTDESC = resp.data.RESULTDESC;
        $scope.RRN = resp.data.RRN;
        $scope.DEALID = resp.data.DEALID;
        $scope.DATETIME = resp.data.DATETIME;

    }, function (err) {
        console.error('ERR', err);
    }).catch(function (err) {
        console.error('ERR', err);
    });


    $scope.cancel = function () {
        $modalInstance.dismiss('cancel');
    }
});