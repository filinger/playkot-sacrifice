using System;

namespace PlaykotSacrificeDesign {
    
    struct Position2D {
        
        Position2D(int x, int y) {
            this.x = x;
            this.y = y;
        }
        
        public int x;
        public int y;
    }
    
    abstract class GameObject {
        
        public Position2D Position { get; protected set; }
    }
    
    interface IMobile {
        
        void Move(Position2D newPosition);
    }
    
    interface IDestructible {
        
        void ApplyDamage(int damage);    
    }
    
    class MobileGameObject : GameObject, IMobile {
        
        public void Move(Position2D newPosition) {
            // Про "телепорт" сами написали. :)
            this.Position = newPosition;
        }
    }
    
    class DestructibleGameObject : GameObject, IDestructible {
        
        private health;
        
        public
    }
    
    class Unit : MobileGameObject {
        
    }
}
