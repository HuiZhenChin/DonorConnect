//const slidePage = document.querySelector(".slide-page");
//const nextBtnFirst = document.querySelector(".firstNext");
//const prevBtnSec = document.querySelector(".prev-1");
//const nextBtnSec = document.querySelector(".next-1");
//const prevBtnThird = document.querySelector(".prev-2");
//const nextBtnThird = document.querySelector(".next-2");
//const prevBtnFourth = document.querySelector(".prev-3");
//const submitBtn = document.querySelector(".submit");
//const progressText = document.querySelectorAll(".step p");
//const progressCheck = document.querySelectorAll(".step .check");
//const bullet = document.querySelectorAll(".step .bullet");
//let current = 1;

//nextBtnFirst.addEventListener("click", function (event) {
//    event.preventDefault();
//    slidePage.style.marginLeft = "-25%";
//    bullet[current - 1].classList.add("active");
//    progressCheck[current - 1].classList.add("active");
//    progressText[current - 1].classList.add("active");
//    current += 1;
//});
//nextBtnSec.addEventListener("click", function (event) {
//    event.preventDefault();
//    slidePage.style.marginLeft = "-50%";
//    bullet[current - 1].classList.add("active");
//    progressCheck[current - 1].classList.add("active");
//    progressText[current - 1].classList.add("active");
//    current += 1;
//});
//nextBtnThird.addEventListener("click", function (event) {
//    event.preventDefault();
//    slidePage.style.marginLeft = "-75%";
//    bullet[current - 1].classList.add("active");
//    progressCheck[current - 1].classList.add("active");
//    progressText[current - 1].classList.add("active");
//    current += 1;
//});
//submitBtn.addEventListener("click", function () {
//    bullet[current - 1].classList.add("active");
//    progressCheck[current - 1].classList.add("active");
//    progressText[current - 1].classList.add("active");
//    current += 1;
//    setTimeout(function () {
//        alert("Your Form Successfully Signed up");
//        location.reload();
//    }, 800);
//});

//prevBtnSec.addEventListener("click", function (event) {
//    event.preventDefault();
//    slidePage.style.marginLeft = "0%";
//    bullet[current - 2].classList.remove("active");
//    progressCheck[current - 2].classList.remove("active");
//    progressText[current - 2].classList.remove("active");
//    current -= 1;
//});
//prevBtnThird.addEventListener("click", function (event) {
//    event.preventDefault();
//    slidePage.style.marginLeft = "-25%";
//    bullet[current - 2].classList.remove("active");
//    progressCheck[current - 2].classList.remove("active");
//    progressText[current - 2].classList.remove("active");
//    current -= 1;
//});
//prevBtnFourth.addEventListener("click", function (event) {
//    event.preventDefault();
//    slidePage.style.marginLeft = "-50%";
//    bullet[current - 2].classList.remove("active");
//    progressCheck[current - 2].classList.remove("active");
//    progressText[current - 2].classList.remove("active");
//    current -= 1;
//});

//document.addEventListener('DOMContentLoaded', (event) => {
//    const steps = document.querySelectorAll('.step');
//    const slidePages = document.querySelectorAll('.slide-page');
//    const nextBtnFirst = document.querySelector(".firstNext");
//    const prevBtnFirst = document.querySelector(".prev-1");
//    const nextBtnSec = document.querySelector(".next-1");
//    const nextBtnThird = document.querySelector(".next-2");
//    const prevBtnSec = document.querySelector(".prev-2");
//    const prevBtnThird = document.querySelector(".prev-3");
//    //const prevBtnThird = document.querySelector(".prev-2");
//    //const nextBtnThird = document.querySelector(".next-3");
//    //const prevBtnFourth = document.querySelector(".prev-3");
//    const finishBtn = document.querySelector('.finish-btn');
//    let current = 0;

//    // Update progress indicators
//    function updateProgress() {
//        steps.forEach((step, index) => {
//            step.classList.toggle('active', index <= current);
//        });
//    }

//    // Show the current slide page
//    function showSlidePage(index) {
//        slidePages.forEach((page, i) => {
//            page.classList.toggle('active', i === index);
//        });
//    }

//    // Handle Next Button Clicks
//    nextBtnFirst.addEventListener("click", function (event) {
//        event.preventDefault();
//        if (current < slidePages.length - 1) {
//            current++;
//            showSlidePage(current);
//            updateProgress();
//        }
//    });

//    nextBtnSec.addEventListener("click", function (event) {
//        event.preventDefault();
//        if (current < slidePages.length - 1) {
//            current++;
//            showSlidePage(current);
//            updateProgress();
//        }
//    });

//    nextBtnThird.addEventListener("click", function (event) {
//        event.preventDefault();
//        if (current < slidePages.length - 1) {
//            current++;
//            showSlidePage(current);
//            updateProgress();
//        }
//    });

//    //nextBtnThird.addEventListener("click", function (event) {
//    //    event.preventDefault();
//    //    if (current < slidePages.length - 1) {
//    //        current++;
//    //        showSlidePage(current);
//    //        updateProgress();
//    //    }
//    //});

//    finishBtn.addEventListener("click", function () {
//        updateProgress();
//        setTimeout(function () {
//            alert("Your Form Successfully Signed Up");
//            location.reload();
//        }, 800);
//    });

//    // Handle Previous Button Clicks
//    prevBtnFirst.addEventListener("click", function (event) {
//        event.preventDefault();
//        if (current > 0) {
//            current--;
//            showSlidePage(current);
//            updateProgress();
//        }
//    });

//    prevBtnSec.addEventListener("click", function (event) {
//        event.preventDefault();
//        if (current > 0) {
//            current--;
//            showSlidePage(current);
//            updateProgress();
//        }
//    });

//    prevBtnThird.addEventListener("click", function (event) {
//        event.preventDefault();
//        if (current > 0) {
//            current--;
//            showSlidePage(current);
//            updateProgress();
//        }
//    });

//    //prevBtnFourth.addEventListener("click", function (event) {
//    //    event.preventDefault();
//    //    if (current > 0) {
//    //        current--;
//    //        showSlidePage(current);
//    //        updateProgress();
//    //    }
//    //});

//    // Initialize
//    showSlidePage(current);
//    updateProgress();
//});

