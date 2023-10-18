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
	});