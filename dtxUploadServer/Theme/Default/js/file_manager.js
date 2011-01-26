var new_item_data = [
	{
		id: 'a',
		preview: 'file:///C:/Projects/2010/C%23/dtxUpload/dtxUploadServer/UI_Tests/images/dtxUploadLogo.png',
		name: 'file_upload number one.jpg',
		modified: null,
		status: 2,
		uploaded: '6:01PM',
		size: 23124,
		views: 21
	},
	{
		id: 'b',
		preview: 'file:///C:/Projects/2010/C%23/dtxUpload/dtxUploadServer/UI_Tests/images/dtxUploadLogo.png',
		name: 'zip number two.zip',
		modified: null,
		status: 2,
		uploaded: '8:21AM',
		size: 23125124,
		views: 261
	}

];

var FileManager = {
	
	$el: {
		tabs: null,
		current_tab: null,
		current_content: null,
		iframes: null
	},
	
	initialize: function(){
		
		// Define elements.
		this.$el.folder_id_default =  $('folder_id_default');
		this.$el.item_id_ = $('item_id_');
		this.$el.items_properties = $('properties');
		this.$el.properties_details_container = $('properties_details_container');
		this.$el.properties_header = $('properties_header');
		this.$el.properties_image = $('properties_image');
		this.$el.properties_buton_selectnone = $('properties_buton_selectnone');
		this.$el.properties = $('properties');
		
		// Setup tween elements.
		this.setupProperties();

		this.insertNewCategory('Public');
		this.insertNewCategory('Private');
		this.insertNewItem('Public', new_item_data[0]);
		this.insertNewItem('Public', new_item_data[1]);
		
		//this.debug();
	},
	
	setupProperties: function(){
		var self = this;
		this.$el.properties_buton_selectnone.addEvent('click', function(){
			$$('input[type=checkbox]').forEach(function(el){
				el.checked = false;
				el.getParent().getParent().removeClass('item_upload-selected');
			});
			self.selected_items_list = [];
			self.updateProperties();
		});
		
		this.$el.properties.set('tween', {
			duration: 300,
			transition: Fx.Transitions.Quint.easeOut
		});
	},
	
	
	// 1 = Uploading; 2 = Uploaded; 3 = Deleted; 4 = Pending Deletion; 5 = Pending Upload; 5 = Disabled;
	status_text: [
		'',
		'Uploading',
		'Uploaded',
		'Deleted',
		'Pending Deletion',
		'Pending Upload',
		'Disabled'
	],
	
	debug: function(){
	
		// Display debug elements.
		$('folder_id_default_upload_1').setStyle('display', '');
		$('folder_id_default').setStyle('display', '');
	},
	
	insertNewCategory: function(cat_name){
		var self = this;
		var hyphen_name = cat_name.replace(' ', '-')
		if(this.$el.folder_id_default == null) return;
		var new_category = this.$el.folder_id_default.clone();
		
		
		new_category.set('id', 'folder_id_' + cat_name);
		new_category.getElement('.folder_name').set('text', cat_name);
		
		// Add the function
		new_category.getElement('.folder_colapse').addEvent('click', function(){
			
			// Determine whether the category is already open or closed.
			var item_visible = new_category.retrieve('items_visible', true);
			
			// Set all the item rows to hide or show.
			self.categoryItemsSetDisplay(new_category, (item_visible)? 'none' : '');
			
			this.set('html', (item_visible)? '+' : '-&nbsp;');
			
			// Invert the value for the items_visible
			new_category.store('items_visible', !item_visible);
			
			window.getSelection().removeAllRanges();
			

		});
		
		// Navigation links setup.
		var nav_links = new_category.getElements('.nav_links');
		
		nav_links[0].addEvent('click', function(){
			self.categoryPreviousPage(hyphen_name);
		});
		
		nav_links[0].addEvent('click', function(){
			self.categoryNextPage(hyphen_name);
		});
		
		// Inject the element into the document.
		new_category.inject($('uploaded_items_list'), 'before');
		new_category.setStyle('display', '');
	},
	
	categoryItemsSetDisplay: function(cat_ele, display_val){
		var items = cat_ele.retrieve('all_items', []);
		for(var i = 0; i < items.length; i++){
			items[i].setStyle('display', display_val);
		}
	},

	
	categoryPreviousPage: function(cat_name){
		var cat_element = $('folder_id_' + cat_name);
	},
	
	categoryNextPage: function(cat_name){
		var cat_element = $('folder_id_' + cat_name);
	},
	
	insertNewItem: function(category, item_data){
		if(this.$el.item_id_ == null) return;
		var self = this;
		var new_item = this.$el.item_id_.clone();
		var parsed_cat_name = category.replace(' ', '-');
		var cat_header = $('folder_id_' + parsed_cat_name);
		var detail_td = new_item.getElements('td');
		var checkbox = detail_td[0].getElement('input');
		
		// Set item events and ID.
		new_item.set({
			id: 'item_id_' + item_data.id,
			
			events:	{
				click: function(){
					var is_checked = checkbox.checked;
					checkbox.checked = !is_checked;
					
					if(is_checked){
						self.onItemDeselect(item_data, new_item);
						
					}else{
						self.onItemSelect(item_data, new_item);
					}
				}
			}
		});
		
		// Checkbox events.
		checkbox.addEvent('click', function(e){
			e.stopPropagation();
			
			if(checkbox.checked){
				self.onItemSelect(item_data, new_item);
				
			}else{
				self.onItemDeselect(item_data, new_item);
			}
		});
		
		// Preview image
		detail_td[1].getElement('img').set('src', item_data.preview);
		
		// File Name
		detail_td[2].getElement('a').set({
			html: item_data.name,
			href: 'http://dtronix.com/dtxUpload/u/' + item_data.name
		});
		
		// Status
		detail_td[3].set('text', this.status_text[item_data.status]);
		
		// Upload Date
		detail_td[4].set('text', item_data.uploaded);
		
		// Size
		detail_td[5].set('text', item_data.size);
		
		// Views
		detail_td[6].set('text', item_data.views);
		
		
		
		var all_upload_items = cat_header.retrieve('all_items', []);
		all_upload_items[all_upload_items.length] = new_item;
		
		// Get the current injection location. If one does not exist, use the category header element.
		new_item.inject(cat_header.retrieve('last_item', cat_header), 'after');
		
		// Set the next injection location.
		cat_header.store('last_item', new_item)
		new_item.setStyle('display', '');
	},
	
	selected_items_list: [],
	
	onItemSelect: function(details, item_element){		
		item_element.addClass('item_upload-selected');
		this.selected_items_list[this.selected_items_list.length] = details;
		
		if(this.selected_items_list.length == 1){
			// Only one item selected at this time.
			
			this.updateProperties();
		}else{
			// Multiple items are selected.
			
			this.updateProperties();
		}
	},
	
	onItemDeselect: function(details, item_element){
		item_element.removeClass('item_upload-selected');
		
		for(var i=0; i < this.selected_items_list.length; i++){
			if(details.id == this.selected_items_list[i].id){
			
				// Found the item to de-select, now remove it.
				this.selected_items_list.splice(i, 1);
				this.updateProperties();
				return;
			}
		}

	},
	
	updateProperties: function(){
	var header, details, image;
		if(this.selected_items_list.length == 1){
			// One item selected.
			
			header = this.selected_items_list[0].name;
			image = this.selected_items_list[0].preview;
			details = [
				'Uploaded: ' + this.selected_items_list[0].uploaded,
				'Size: ' + this.selected_items_list[0].size,
				'Views: ' + this.selected_items_list[0].views
			];
		}else if(this.selected_items_list.length > 1){
			// Multiple items selected.
			
			header = this.selected_items_list.length + ' Selected Items';
			image = this.selected_items_list[0].preview;
			
		}else{
			this.$el.properties.tween('bottom', -61);
			return;
		}
		
		this.$el.properties.tween('bottom', 0);
		var self = this;
		this.$el.properties_header.innerHTML = header;
		
		if(image != null){
			this.$el.properties_image.src = image;
		}
		
		// Clear out all the old properties.
		this.$el.properties_details_container.empty();
		
		if(details != undefined){
		
			// Add all the new ones.
			details.forEach(function(detail){
				new Element('div', {
					class: 'properties_details',
					text: detail
				}).inject(self.$el.properties_details_container);
			})
		}
		
	}
	
};


window.addEvent('domready', function() {
	FileManager.initialize();
});