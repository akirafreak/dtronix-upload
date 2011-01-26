var main = {

	$el: {
		tabs: null,
		current_tab: null,
		current_content: null,
		iframes: null
	},	
	
	initialize: function(){
		this.setupTabs();
		this.fixIframeHeights();
		
		window.addEvent('resize', this.fixIframeHeights.bind(this))
	},
	
	fixIframeHeights: function(){
		if(this.$el.iframes == null){
			this.$el.iframes = $$('iframe');
		}
		var height = window.getSize().y;
		var minus_height = $('tab_container').getSize().y;
		
		this.$el.iframes.each(function(el){
			el.setStyle('height',  height - minus_height - 6);
		});
	},
	
	setupTabs: function(){
		this.$el.tabs = $$('.section_tab_container');
		var self = this;
		this.$el.tabs.each(function(el){
			
			// Do not add this functionality to the search bar.
			if(el.hasClass('section_tab_container_search')) return;
			
			// Setup the tween on each element.
			el.set('tween', {
				'duration': 400
			});
			
			// Add the fade effect to the elements.
			el.addEvents({
				'mouseover': function(){
					if(!this.hasClass('section_tab_container-selected')){
						// Ensure the tab is not already selected.
						
						this.tween('background-color', '#5c5c5c');
					}
				},
				
				'mouseout': function(){
					if(!this.hasClass('section_tab_container-selected')){
						// Ensure the tab is not already selected.
						
						this.tween('background-color', '#383838');
					}
				},
				
				'click': function(){
					var tab_name = this.getFirst().innerHTML.clean().toLowerCase();
					var content_el =  $('content_' + tab_name);
					
					// Ensure the tab is not already selected.
					if(self.$el.current_tab == null || (!this.hasClass('section_tab_container-selected') && content_el != null)){
						
						this.get('tween').cancel();
						this.setStyle('background-color', '');
						this.addClass('section_tab_container-selected');
						
						// If we already have a tab selected, un-select it and hide the displayed content.
						if(self.$el.current_tab != null) self.$el.current_tab.removeClass('section_tab_container-selected');
						if(self.$el.currnet_content != null) self.$el.currnet_content.setStyle('display', 'none');
						
						// Update the vars.
						self.$el.current_tab = this;
						self.$el.currnet_content = content_el;
						
						// Show the proper content.
						content_el.setStyle('display', '');
					}
				}
			});
			
			if(el.hasClass('section_tab_container-selected')){
				el.fireEvent('click');
			}
		});
	}
	
};


window.addEvent('domready', function() {
    main.initialize();
});

