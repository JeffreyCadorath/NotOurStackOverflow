let optionList = document.querySelectorAll("option")

optionList.forEach(option => {
    option.addEventListener("mousedown", e => {
        e.preventDefault();
        let target = e.target;
        if (target.getAttribute("selected") == null || target.getAttribute("selected") === false) {
            target.setAttribute("selected", true)
        } else {
            target.removeAttribute("selected")
        }
        return;
    })
})
//csHtml for MultiSelectList
//   @Html.ListBox("values", (MultiSelectList)ViewBag.ListTest)

//Controller C#  note: 
//   ViewBag.ListTest = new MultiSelectList(List<T> List of items, (string)ListItemValue, (string)ListItemName);