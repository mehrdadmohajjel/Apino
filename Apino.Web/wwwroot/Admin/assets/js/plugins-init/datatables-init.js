let dataSet = [
    [ "رضا محمدی", "معمار سیستم", "تهران", "5421", "1400/02/05", "۳۲۰,۸۰۰,۰۰۰ تومان" ],
    [ "احمد حسینی", "حسابدار", "مشهد", "8422", "1400/05/05", "۱۷۰,۷۵۰,۰۰۰ تومان" ],
    [ "محمد کریمی", "نویسنده فنی", "اصفهان", "1562", "1397/10/22", "۸۶,۰۰۰,۰۰۰ تومان" ],
    [ "علی نجفی", "توسعه‌دهنده ارشد جاوااسکریپت", "تهران", "6224", "1401/01/10", "۴۳۳,۰۶۰,۰۰۰ تومان" ],
    [ "فاطمه زاهدی", "حسابدار", "شیراز", "5407", "1397/08/08", "۱۶۲,۷۰۰,۰۰۰ تومان" ],
    [ "مریم قربانی", "متخصص یکپارچه‌سازی", "تبریز", "4804", "1401/09/12", "۳۷۲,۰۰۰,۰۰۰ تومان" ],
    [ "حسین امینی", "دستیار فروش", "کرج", "9608", "1401/05/16", "۱۳۷,۵۰۰,۰۰۰ تومان" ],
    [ "زهرا موسوی", "متخصص یکپارچه‌سازی", "اهواز", "6200", "1399/07/23", "۳۲۷,۹۰۰,۰۰۰ تومان" ],
    [ "مهدی رضایی", "توسعه‌دهنده جاوااسکریپت", "قم", "2360", "1398/06/25", "۲۰۵,۵۰۰,۰۰۰ تومان" ],
    [ "سارا جعفری", "مهندس نرم‌افزار", "رشت", "1667", "1397/09/23", "۱۰۳,۶۰۰,۰۰۰ تومان" ],
    [ "نازنین احمدی", "مدیر دفتر", "کرمانشاه", "3814", "1397/09/29", "۹۰,۵۶۰,۰۰۰ تومان" ],
    [ "امیر عباسی", "رئیس پشتیبانی", "یزد", "9497", "1401/12/13", "۳۴۲,۰۰۰,۰۰۰ تومان" ],
    [ "لیلا محمودی", "مدیر منطقه‌ای", "ارومیه", "6741", "1397/07/25", "۴۷۰,۶۰۰,۰۰۰ تومان" ],
    [ "پارسا کرمانی", "طراح ارشد بازاریابی", "زنجان", "3597", "1401/09/28", "۳۱۳,۵۰۰,۰۰۰ تومان" ],
    [ "نگین صالحی", "مدیر منطقه‌ای", "بندرعباس", "1965", "1398/12/26", "۳۸۵,۷۵۰,۰۰۰ تومان" ],
    [ "کامبیز طاهری", "طراح بازاریابی", "قزوین", "1581", "1401/08/07", "۱۹۸,۵۰۰,۰۰۰ تومان" ],
    [ "حمید مرادی", "مدیر مالی ارشد", "ساری", "3059", "1399/03/20", "۷۲۵,۰۰۰,۰۰۰ تومان" ],
    [ "الهام نوری", "مدیر سیستم", "بوشهر", "1721", "1398/01/21", "۲۳۷,۵۰۰,۰۰۰ تومان" ],
    [ "بابک زمانی", "مهندس نرم‌افزار", "کرج", "2558", "1401/07/22", "۱۳۲,۰۰۰,۰۰۰ تومان" ],
    [ "دانیال هاشمی", "رئیس پرسنل", "همدان", "2290", "1401/06/05", "۲۱۷,۵۰۰,۰۰۰ تومان" ],
    [ "شیما قاسمی", "رئیس توسعه", "اصفهان", "1937", "1400/06/12", "۳۴۵,۰۰۰,۰۰۰ تومان" ],
    [ "یاسمن بابایی", "مدیر ارشد بازاریابی", "مشهد", "6154", "1398/04/05", "۶۷۵,۰۰۰,۰۰۰ تومان" ],
    [ "فرید رنجبر", "پشتیبانی پیش از فروش", "تبریز", "8330", "1400/09/21", "۱۰۶,۴۵۰,۰۰۰ تومان" ],
    [ "نرگس امیرخانی", "دستیار فروش", "رشت", "3023", "1399/06/29", "۸۵,۶۰۰,۰۰۰ تومان" ],
    [ "آرمان سلطانی", "مدیر عامل", "تهران", "5797", "1398/07/17", "۱,۲۰۰,۰۰۰,۰۰۰ تومان" ],
    [ "پویا محمدرضایی", "توسعه‌دهنده", "قم", "8822", "1399/10/01", "۹۲,۵۷۵,۰۰۰ تومان" ],
    [ "سمیرا علیزاده", "مدیر منطقه‌ای", "شیراز", "9239", "1399/08/23", "۳۵۷,۶۵۰,۰۰۰ تومان" ],
    [ "امیدوار رضوی", "مهندس نرم‌افزار", "کرمان", "1314", "1400/03/17", "۲۰۶,۸۵۰,۰۰۰ تومان" ],
    [ "نگار سلیمانی", "مدیر عملیات", "اهواز", "2947", "1398/12/20", "۸۵۰,۰۰۰,۰۰۰ تومان" ],
    [ "شایان محسنی", "بازاریاب منطقه‌ای", "یزد", "8899", "1400/05/23", "۱۶۳,۰۰۰,۰۰۰ تومان" ],
    [ "مینا کاظمی", "متخصص یکپارچه‌سازی", "بندرعباس", "2769", "1400/03/12", "۹۵,۴۰۰,۰۰۰ تومان" ],
    [ "سپهر مقدسی", "توسعه‌دهنده", "زاهدان", "6832", "1398/07/30", "۱۱۴,۵۰۰,۰۰۰ تومان" ],
    [ "پرهام نوروزی", "نویسنده فنی", "گرگان", "3606", "1400/02/17", "۱۴۵,۰۰۰,۰۰۰ تومان" ],
    [ "رهام اسلامی", "رهبر تیم", "سمنان", "2860", "1397/07/04", "۲۳۵,۵۰۰,۰۰۰ تومان" ],
    [ "لاله فرهادی", "پشتیبانی پس از فروش", "اراک", "8240", "1400/12/18", "۳۲۴,۰۵۰,۰۰۰ تومان" ],
    [ "نیما یوسفی", "طراح بازاریابی", "خرم‌آباد", "5384", "1398/09/18", "۸۵,۶۷۵,۰۰۰ تومان" ]
];


(function($) {
    "use strict"
	
	if(jQuery('#tableZeroConfig').length > 0 ){
		$('#tableZeroConfig').DataTable();
	}
	
	if(jQuery('#tableResponsive').length > 0 ){
		$('#tableResponsive').DataTable({		
			responsive: true,
		});
	}
	
	
	
	
	
	
	
	
	
	//example 2
    /* var table2 = $('#example2').DataTable( {
        createdRow: function ( row, data, index ) {
            $(row).addClass('selected')
        },

        "scrollY":        "42vh",
        "scrollCollapse": true,
        "paging":         false
    });

    table2.on('click', 'tbody tr', function() {
        var $row = table2.row(this).nodes().to$();
        var hasClass = $row.hasClass('selected');
        if (hasClass) {
            $row.removeClass('selected')
        } else {
            $row.addClass('selected')
        }
    })
        
    table2.rows().every(function() {
        this.nodes().to$().removeClass('selected')
    });
	
	// dataTable1
	var table = $('#dataTable1').DataTable({
		searching: false,
		paging:true,
		select: false,         
		lengthChange:false ,
		
	});
	// dataTable2
	var table = $('#dataTable2').DataTable({
		searching: false,
		paging:true,
		select: false,         
		lengthChange:false ,
		
	});
	// dataTable3
	var table = $('#dataTable3').DataTable({
		searching: false,
		paging:true,
		select: false,         
		lengthChange:false ,
		
	});
	// dataTable4
	var table = $('#dataTable4').DataTable({
		searching: false,
		paging:true,
		select: false,         
		lengthChange:false,
		
	});
   
	// dataTable5
	var table = $('#example5').DataTable({
		searching: false,
		paging:false,
		select: false,
		info: false,         
		lengthChange:false ,
		language: {
			paginate: {
			  next: '<i class="fa-solid fa-angle-right"></i>',
			  previous: '<i class="fa-solid fa-angle-left"></i>' 
			}
		  }
		
	}); 
	
	// dataTable6
		var table = $('#example6').DataTable({
			searching: false,
			paging:true,
			select: false,
			info: false,         
			lengthChange:false ,
			language: {
			paginate: {
			  next: '<i class="fa-solid fa-angle-right"></i>',
			  previous: '<i class="fa-solid fa-angle-left"></i>' 
			}
		  }
			
		});
		
		
	// dataTable7
	var table = $('#example7').DataTable({
		searching: false,
		paging:true,
		select: false,
		info: true,         
		lengthChange:false ,
		language: {
			paginate: {
			   next: '<i class="fa-solid fa-angle-right"></i>',
			  previous: '<i class="fa-solid fa-angle-left"></i>' 
			}
		  }
		
	}); 	
	// dataTable9
		
	// table row
	var table = $('#dataTable1, #dataTable2, #dataTable3, #dataTable4,  #example3, #example4').DataTable({
		language: {
			paginate: {
			  next: '<i class="fa-solid fa-angle-right"></i>',
			  previous: '<i class="fa-solid fa-angle-left"></i>' 
			}
		  }
	});
	$('#example tbody').on('click', 'tr', function () {
		var data = table.row( this ).data();
	});
   
	// application table
	var table = $('#application-tbl1,#application-tbl2,#application-tbl3,#application-tbl4 ').DataTable({
		searching: false,
		lengthChange:false ,
		language: {
			paginate: {
			  next: '<i class="fa-solid fa-angle-right"></i>',
			  previous: '<i class="fa-solid fa-angle-left"></i>' 
			}
		  }
	}); */
	
})(jQuery);