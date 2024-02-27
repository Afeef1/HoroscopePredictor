function formatDateWithoutLeadingZeroes(date) {
    const day = date.getDate().toString();
    const month = (date.getMonth() + 1).toString(); // Months are zero-based, so we add 1
    const year = date.getFullYear().toString();
  
    return `${day}-${month}-${year}`;
  }

  module.exports= {formatDateWithoutLeadingZeroes};