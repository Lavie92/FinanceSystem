var cancel = document.getElementById("cancel");
var adddatedetails = document.getElementById("adddatedetails");

cancel.addEventListener("click", function() {
  if (adddatedetails.style.display === "none") {
    adddatedetails.style.display = "block"; // Hiển thị phần tử
  } else {
    adddatedetails.style.display = "none"; // Ẩn phần tử đi
  }
});