/*7. Init Price Slider

	*/
	jQuery(document).ready(function ($) {
		"use strict";
		initFavorite();
		initFixProductBorder();
		initIsotopeFiltering();
		initPriceSlider();
		initCheckboxes();
		function initFavorite()
		{
			if($('.sec_categories .favorite').length)
			{
				var favs = $('.sec_categories .favorite');
	
				favs.each(function()
				{
					var fav = $(this);
					var active = false;
					if(fav.hasClass('active'))
					{
						active = true;
					}
	
					fav.on('click', function()
					{
						if(active)
						{
							fav.removeClass('active');
							active = false;
						}
						else
						{
							fav.addClass('active');
							active = true;
						}
					});
				});
			}
		}
	
		/* 
	
		5. Init Fix Product Border
	
		*/
	
		function initFixProductBorder()
		{
			if($('.sec_categories .product_filter').length)
			{
				var products = $('.sec_categories .product_filter:visible');
				var wdth = window.innerWidth;
	
				// reset border
				products.each(function()
				{
					$(this).css('border-right', 'solid 1px #e9e9e9');
				});
	
				// if window width is 991px or less
	
				if(wdth < 480)
				{
					for(var i = 0; i < products.length; i++)
					{
						var product = $(products[i]);
						product.css('border-right', 'none');
					}
				}
	
				else if(wdth < 576)
				{
					if(products.length < 5)
					{
						var product = $(products[products.length - 1]);
						product.css('border-right', 'none');
					}
					for(var i = 1; i < products.length; i+=2)
					{
						var product = $(products[i]);
						product.css('border-right', 'none');
					}
				}
	
				else if(wdth < 768)
				{
					if(products.length < 5)
					{
						var product = $(products[products.length - 1]);
						product.css('border-right', 'none');
					}
					for(var i = 2; i < products.length; i+=3)
					{
						var product = $(products[i]);
						product.css('border-right', 'none');
					}
				}
	
				else if(wdth < 992)
				{
					if(products.length < 5)
					{
						var product = $(products[products.length - 1]);
						product.css('border-right', 'none');
					}
					for(var i = 2; i < products.length; i+=3)
					{
						var product = $(products[i]);
						product.css('border-right', 'none');
					}
				}
	
				//if window width is larger than 991px
				else
				{
					if(products.length < 5)
					{
						var product = $(products[products.length - 1]);
						product.css('border-right', 'none');
					}
					for(var i = 3; i < products.length; i+=4)
					{
						var product = $(products[i]);
						product.css('border-right', 'none');
					}
				}	
			}
		}
	
		/* 
	
		6. Init Isotope Filtering
	
		*/
	
		function initIsotopeFiltering()
		{
			var sortTypes = $('.sec_categories .type_sorting_btn');
			var sortNums = $('.sec_categories .num_sorting_btn');
			var sortTypesSelected = $('.sec_categories .sorting_type .item_sorting_btn is-checked span');
			var filterButton = $('.sec_categories .filter_button');
	
			if($('.sec_categories .product-grid').length)
			{
				$('.sec_categories .product-grid').isotope({
					itemSelector: '.product-item',
					getSortData: {
						price: function(itemElement)
						{
							var priceEle = $(itemElement).find('.sec_categories .product_price').text().replace( '$', '' );
							return parseFloat(priceEle);
						},
						name: '.sec_categories .product_name'
					},
					animationOptions: {
						duration: 750,
						easing: 'linear',
						queue: false
					}
				});
	
				// Short based on the value from the sorting_type dropdown
				sortTypes.each(function()
				{
					$(this).on('click', function()
					{
						$('.sec_categories .type_sorting_text').text($(this).text());
						var option = $(this).attr('data-isotope-option');
						option = JSON.parse( option );
						$('.sec_categories .product-grid').isotope( option );
					});
				});
	
				// Show only a selected number of items
				sortNums.each(function()
				{
					$(this).on('click', function()
					{
						var numSortingText = $(this).text();
						var numFilter = ':nth-child(-n+' + numSortingText + ')';
						$('.sec_categories .num_sorting_text').text($(this).text());
						$('.sec_categories .product-grid').isotope({filter: numFilter });
					});
				});	
	
				// Filter based on the price range slider
				filterButton.on('click', function()
				{
					$('.sec_categories .product-grid').isotope({
						filter: function()
						{
							var priceRange = $('#amount').val();
							var priceMin = parseFloat(priceRange.split('-')[0].replace('$', ''));
							var priceMax = parseFloat(priceRange.split('-')[1].replace('$', ''));
							var itemPrice = $(this).find('.sec_categories .product_price').clone().children().remove().end().text().replace( '$', '' );
	
							return (itemPrice > priceMin) && (itemPrice < priceMax);
						},
						animationOptions: {
							duration: 750,
							easing: 'linear',
							queue: false
						}
					});
				});
			}
		}
	
		/* 
	
		7. Init Price Slider
	
		*/
	
		function initPriceSlider()
		{
			$( "#slider-range" ).slider(
			{
				range: true,
				min: 0,
				max: 1000,
				values: [ 0, 580 ],
				slide: function( event, ui )
				{
					$( "#amount" ).val( "$" + ui.values[ 0 ] + " - $" + ui.values[ 1 ] );
				}
			});
				
			$( "#amount" ).val( "$" + $( "#slider-range" ).slider( "values", 0 ) + " - $" + $( "#slider-range" ).slider( "values", 1 ) );
		}
	
		/* 
	
		8. Init Checkboxes
	
		*/
	
		function initCheckboxes()
		{
			if($('.sec_categories .checkboxes li').length)
			{
				var boxes = $('.sec_categories .checkboxes li');
	
				boxes.each(function()
				{
					var box = $(this);
	
					box.on('click', function()
					{
						if(box.hasClass('active'))
						{
							box.find('i').removeClass('fa-square');
							box.find('i').addClass('fa-square-o');
							box.toggleClass('active');
						}
						else
						{
							box.find('i').removeClass('fa-square-o');
							box.find('i').addClass('fa-square');
							box.toggleClass('active');
						}
						// box.toggleClass('active');
					});
				});
	
				if($('.sec_categories .show_more').length)
				{
					var checkboxes = $('.sec_categories .checkboxes');
	
					$('.sec_categories .show_more').on('click', function()
					{
						checkboxes.toggleClass('active');
					});
				}
			};
		}
	});  jQuery(document).ready(function ($) {
	"use strict";
	initThumbnail();
	initQuantity();
	initStarRating();
	initTabs();
	function initThumbnail() {
		if ($('.single_product_thumbnails ul li').length) {
			var thumbs = $('.single_product_thumbnails ul li');
			var singleImage = $('.single_product_image_background');

			thumbs.each(function () {
				var item = $(this);
				item.on('click', function () {
					thumbs.removeClass('active');
					item.addClass('active');
					var img = item.find('img').data('image');
					singleImage.css('background-image', 'url(' + img + ')');
				});
			});
		}
	}

	/* 

	5. Init Quantity

	*/

	function initQuantity() {
		if ($('.plus').length && $('.minus').length) {
			var plus = $('.plus');
			var minus = $('.minus');
			var value = $('#quantity_value');

			plus.on('click', function () {
				var x = parseInt(value.text());
				value.text(x + 1);
			});

			minus.on('click', function () {
				var x = parseInt(value.text());
				if (x > 1) {
					value.text(x - 1);
				}
			});
		}
	}

	/* 

	6. Init Star Rating

	*/

	function initStarRating() {
		if ($('.user_star_rating li').length) {
			var stars = $('.user_star_rating li');

			stars.each(function () {
				var star = $(this);

				star.on('click', function () {
					var i = star.index();

					stars.find('i').each(function () {
						$(this).removeClass('fa-star');
						$(this).addClass('fa-star-o');
					});
					for (var x = 0; x <= i; x++) {
						$(stars[x]).find('i').removeClass('fa-star-o');
						$(stars[x]).find('i').addClass('fa-star');
					};
				});
			});
		}
	}
	function initTabs() {
		if ($('.tabs').length) {
			var tabs = $('.tabs li');
			var tabContainers = $('.tab_container');

			tabs.each(function () {
				var tab = $(this);
				var tab_id = tab.data('active-tab');

				tab.on('click', function () {
					if (!tab.hasClass('active')) {
						tabs.removeClass('active');
						tabContainers.removeClass('active');
						tab.addClass('active');
						$('#' + tab_id).addClass('active');
					}
				});
			});
		}
	}
}); /* JS Document */

/******************************

[Table of Contents]

1. Vars and Inits
2. Set Header
3. Init Menu
4. Init Timer
5. Init Favorite
6. Init Fix Product Border
7. Init Isotope Filtering
8. Init Slider


******************************/

jQuery(document).ready(function ($) {
	"use strict";

	/* 

	1. Vars and Inits

	*/

	var header = $('.header');
	var topNav = $('.top_nav')
	var mainSlider = $('.main_slider');
	var hamburger = $('.hamburger_container');
	var menu = $('.hamburger_menu');
	var menuActive = false;
	var hamburgerClose = $('.hamburger_close');
	var fsOverlay = $('.fs_menu_overlay');
	var top = $('#dmtop');
	setHeader();

	$(window).on('resize', function () {
		initFixProductBorder();
		setHeader();
	});

	$(document).on('scroll', function () {
		setHeader();
		scrolltoTop();
	});

	initMenu();
	initTimer();
	initFavorite();
	initFixProductBorder();
	initIsotopeFiltering();
	initSlider();

	function scrolltoTop() {

		if ($(window).scrollTop() > 100) {
			top.css({ 'bottom': "120px" });
		}
		else {
			top.css({ 'bottom': "-100px" });
		}

	}
	top.on('click', function () {
		$([document.documentElement, document.body]).animate({ scrollTop: 0 }, 800);
	});
	/* 

	2. Set Header

	*/

	function setHeader() {
		if (window.innerWidth < 992) {
			if ($(window).scrollTop() > 100) {
				header.css({ 'top': "0" });
			}
			else {
				header.css({ 'top': "0" });
			}
		}
		else {

			header.css({ 'top': "0" });

		}
		if (window.innerWidth > 991 && menuActive) {
			closeMenu();
		}
	}

	/* 

	3. Init Menu

	*/

	function initMenu() {
		if (hamburger.length) {
			hamburger.on('click', function () {
				if (!menuActive) {
					openMenu();
				}
			});
		}

		if (fsOverlay.length) {
			fsOverlay.on('click', function () {
				if (menuActive) {
					closeMenu();
				}
			});
		}

		if (hamburgerClose.length) {
			hamburgerClose.on('click', function () {
				if (menuActive) {
					closeMenu();
				}
			});
		}

		if ($('.menu_item').length) {
			var items = document.getElementsByClassName('menu_item');
			var i;

			for (i = 0; i < items.length; i++) {
				if (items[i].classList.contains("has-children")) {
					items[i].onclick = function () {
						this.classList.toggle("active");
						var panel = this.children[1];
						if (panel.style.maxHeight) {
							panel.style.maxHeight = null;
						}
						else {
							panel.style.maxHeight = panel.scrollHeight + "px";
						}
					}
				}
			}
		}
	}

	function openMenu() {
		menu.addClass('active');
		// menu.css('right', "0");
		fsOverlay.css('pointer-events', "auto");
		menuActive = true;
	}

	function closeMenu() {
		menu.removeClass('active');
		fsOverlay.css('pointer-events', "none");
		menuActive = false;
	}

	/* 

	4. Init Timer

	*/

	function initTimer() {
		if ($('.timer').length) {
			// Uncomment line below and replace date
			// var target_date = new Date("Dec 7, 2017").getTime();

			// comment lines below
			var date = new Date();
			date.setDate(date.getDate() + 3);
			var target_date = date.getTime();
			//----------------------------------------

			// variables for time units
			var days, hours, minutes, seconds;

			var d = $('#day');
			var h = $('#hour');
			var m = $('#minute');
			var s = $('#second');

			setInterval(function () {
				// find the amount of "seconds" between now and target
				var current_date = new Date().getTime();
				var seconds_left = (target_date - current_date) / 1000;

				// do some time calculations
				days = parseInt(seconds_left / 86400);
				seconds_left = seconds_left % 86400;

				hours = parseInt(seconds_left / 3600);
				seconds_left = seconds_left % 3600;

				minutes = parseInt(seconds_left / 60);
				seconds = parseInt(seconds_left % 60);

				// display result
				d.text(days);
				h.text(hours);
				m.text(minutes);
				s.text(seconds);

			}, 1000);
		}
	}

	/* 

	5. Init Favorite

	*/

	function initFavorite() {
		if ($('.favorite').length) {
			var favs = $('.favorite');

			favs.each(function () {
				var fav = $(this);
				var active = false;
				if (fav.hasClass('active')) {
					active = true;
				}

				fav.on('click', function () {
					if (active) {
						fav.removeClass('active');
						active = false;
					}
					else {
						fav.addClass('active');
						active = true;
					}
				});
			});
		}
	}

	/* 

	6. Init Fix Product Border

	*/

	function initFixProductBorder() {
		if ($('.product_filter').length) {
			var products = $('.product_filter:visible');
			var wdth = window.innerWidth;

			// reset border
			products.each(function () {
				$(this).css('border-right', 'solid 1px #e9e9e9');
			});

			// if window width is 991px or less

			if (wdth < 480) {
				for (var i = 0; i < products.length; i++) {
					var product = $(products[i]);
					product.css('border-right', 'none');
				}
			}

			else if (wdth < 576) {
				if (products.length < 5) {
					var product = $(products[products.length - 1]);
					product.css('border-right', 'none');
				}
				for (var i = 1; i < products.length; i += 2) {
					var product = $(products[i]);
					product.css('border-right', 'none');
				}
			}

			else if (wdth < 768) {
				if (products.length < 5) {
					var product = $(products[products.length - 1]);
					product.css('border-right', 'none');
				}
				for (var i = 2; i < products.length; i += 3) {
					var product = $(products[i]);
					product.css('border-right', 'none');
				}
			}

			else if (wdth < 992) {
				if (products.length < 5) {
					var product = $(products[products.length - 1]);
					product.css('border-right', 'none');
				}
				for (var i = 3; i < products.length; i += 4) {
					var product = $(products[i]);
					product.css('border-right', 'none');
				}
			}

			//if window width is larger than 991px
			else {
				if (products.length < 5) {
					var product = $(products[products.length - 1]);
					product.css('border-right', 'none');
				}
				for (var i = 4; i < products.length; i += 5) {
					var product = $(products[i]);
					product.css('border-right', 'none');
				}
			}
		}
	}

	/* 

	7. Init Isotope Filtering

	*/

	function initIsotopeFiltering() {
		if ($('.grid_sorting_button').length) {
			$('.grid_sorting_button').click(function () {
				// putting border fix inside of setTimeout because of the transition duration
				setTimeout(function () {
					initFixProductBorder();
				}, 500);

				$('.grid_sorting_button.active').removeClass('active');
				$(this).addClass('active');

				var selector = $(this).attr('data-filter');
				$('.product-grid').isotope({
					filter: selector,
					animationOptions: {
						duration: 750,
						easing: 'linear',
						queue: false
					}
				});


				return false;
			});
		}
	}

	/* 

	8. Init Slider

	*/

	function initSlider() {
		if ($('.product_slider').length) {
			var slider1 = $('.product_slider');

			slider1.owlCarousel({
				loop: false,
				dots: false,
				nav: false,
				responsive:
				{
					0: { items: 1 },
					480: { items: 2 },
					768: { items: 3 },
					991: { items: 4 },
					1280: { items: 5 },
					1440: { items: 5 }
				}
			});

			if ($('.product_slider_nav_left').length) {
				$('.product_slider_nav_left').on('click', function () {
					slider1.trigger('prev.owl.carousel');
				});
			}

			if ($('.product_slider_nav_right').length) {
				$('.product_slider_nav_right').on('click', function () {
					slider1.trigger('next.owl.carousel');
				});
			}
		}
	}
});