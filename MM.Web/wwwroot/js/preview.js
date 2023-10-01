
//function () {
//	let rowCounter = 1;
//	var rowCount = $("#myTableBody tr").length;
//	var newRow = $('<tr>');
//	var cols = '';
//	rowCounter++;
//	cols += '<td><input name="row-' + rowCounter + '_from" type="text" class="from-input" placeholder="From"></td>';
//	cols += '<td><input name="row-' + rowCounter + '_to" type="text" class="to-input" placeholder="To"></td>';
//	cols += '<td><input name="row-' + rowCounter + '_qty" type="text" class="qty-input" placeholder="Quantity" readonly></td>';



//	newRow.append(cols);
//	newRow.attr('id', 'row-' + (rowCounter)); // add id attribute with an incrementing number
//	$('tbody').append(newRow);

//	if (rowCount > 0) {
//		$("#myPreviewTableBody tr:last-child .remove-row").show();
//	}
//}