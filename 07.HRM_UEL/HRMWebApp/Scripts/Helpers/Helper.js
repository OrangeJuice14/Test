Array.prototype.ToTree = (textParentId,textId,textChild) => {
	var map = {}, node, roots = [], i;
	for (i = 0; i < this.length; i += 1) {
		map[this[i][Id]] = i; // initialize the map
		this[i][textChild] = []; // initialize the children
	}
	for (i = 0; i < this.length; i += 1) {
		node = this[i];

		if (node[textParentId] != null) {
			// if you have dangling branches check that map[node.parentId] exists
			this[map[node[textParentId]]][textChild].push(node);
			this[map[node[textParentId]]].expanded = true;
		} else {
			roots.push(node);
		}
	}
	return roots;
}

Math.Round = (value, exp) => {
	//var value = this;
	if (typeof exp === 'undefined' || +exp === 0) {
      return Math.round(value);
    }
    value = +value;
    exp = +exp;
    // Nếu value không phải là ố hoặc exp không phải là số nguyên thì...
    if (isNaN(value) || !(typeof exp === 'number' && exp % 1 === 0)) {
      return NaN;
    }
    // Shift
    value = value.toString().split('e');
    value = Math.round(+(value[0] + 'e' + (value[1] ? (+value[1] - exp) : -exp)));
    // Shift back
    value = value.toString().split('e');
    return +(value[0] + 'e' + (value[1] ? (+value[1] + exp) : exp));
}