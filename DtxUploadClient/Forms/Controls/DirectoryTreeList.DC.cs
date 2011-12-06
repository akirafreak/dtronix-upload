using System;
using System.Collections.Generic;
using System.Text;

namespace DtxUpload {

	/// <summary>
	/// Current status of the control.
	/// </summary>
	public enum DirectoryTreeListStatusEnum {
		idle,

		creating,
		created,
		create_error,

		renaming,
		renamed,
		rename_error,

		deleting,
		deleted,
		delete_error,

		property_setting,
		property_set,
		property_set_error,
	}
}
