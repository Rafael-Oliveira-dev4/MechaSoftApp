using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MechaSoft.Domain.Interfaces;

public interface IAuditable
{
    DateTime? CreatedAt { get; set; }
    DateTime? UpdatedAt { get; set; }
    string? CreatedBy { get; set; }
    string? UpdatedBy { get; set; }
    bool IsDeleted { get; set; }

    void MarkAsDeleted(string? deletedBy = null);
    void UpdateTimestamp(string? updatedBy = null);
}