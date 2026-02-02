$(function () {
	
	if(jQuery('#example1').length > 0 ){
		$('#example1').multiselectsplitter();		
	}
    
	if(jQuery('#example2').length > 0 ){
		$('#example2').multiselectsplitter({
			selectSize: 7,
			clearOnFirstChange: true,
			groupCounter: true
		});
	}
    
	if(jQuery('#example3').length > 0 ){
		$('#example3').multiselectsplitter({
			groupCounter: true,
			maximumSelected: 2
		});
	}
    
	if(jQuery('#example4').length > 0 ){
		$('#example4').multiselectsplitter({
			groupCounter: true,
			maximumSelected: 3,
			onlySameGroup: true
		});
	}
	
	if(jQuery('#example5').length > 0 ){
    $('#example5').multiselectsplitter({
        size: 6,
        groupCounter: true,
        maximumSelected: 2,
        maximumAlert: function(maximumSelected) {
            alert('شما ' + (maximumSelected + 1) + ' گزینه انتخاب کرده‌اید. آیا مطمئن هستید؟');
        },
        afterInitialize: function($firstSelect, $secondSelect) {
            $("body").append('این متن پس از مقداردهی اولیه example5 اضافه شد');
        },
        createFirstSelect: function (label, $originalSelect) { 
            return '<option class="text-success">پیشوند - ' + label + '</option>';
        },
        createSecondSelect: function (label, $firstSelect) { 
            return '<option class="text-danger"> ??? </option>';
        }
    });
}
    
});