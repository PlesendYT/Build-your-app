import React, { useState } from 'react';
import { View, StyleSheet, Dimensions, PanResponder } from 'react-native';
import GameEngine from 'react-native-game-engine';

const { width, height } = Dimensions.get('window');

export default function App() {
  const [entities, setEntities] = useState({
    player: {
      position: { x: width / 2, y: height - 80 },
      size: 30,
      renderer: () => null,
    },
    coin: {
      position: { x: width / 2, y: 50 },
      size: 20,
      collected: false,
      renderer: () => null,
    },
  });

  const panResponder = PanResponder.create({
    onStartShouldSetPanResponder: () => true,
    onPanResponderMove: (_, g) => {
      setEntities(prev => ({
        ...prev,
        player: { ...prev.player, position: { x: g.moveX, y: g.moveY } },
      }));
    },
  });

  const updateSystem = (entities, { touches }) => {
    const player = entities.player;
    const coin = entities.coin;

    if (!coin.collected) {
      const dx = player.position.x - coin.position.x;
      const dy = player.position.y - coin.position.y;
      if (Math.sqrt(dx * dx + dy * dy) < player.size + coin.size) {
        return { ...entities, coin: { ...coin, collected: true } };
      }
    }
    return entities;
  };

  return (
    <View style={styles.container} {...panResponder.panHandlers}>
      <GameEngine
        systems={[updateSystem]}
        entities={entities}
        renderer={(entities) => (
          <>
            <View
              style={[styles.player, {
                left: entities.player.position.x - entities.player.size / 2,
                top: entities.player.position.y - entities.player.size / 2,
              }]}
            />
            {!entities.coin.collected && (
              <View
                style={[styles.coin, {
                  left: entities.coin.position.x - entities.coin.size / 2,
                  top: entities.coin.position.y - entities.coin.size / 2,
                }]}
              />
            )}
          </>
        )}
      />
    </View>
  );
}

const styles = StyleSheet.create({
  container: { flex: 1, backgroundColor: '#fff' },
  player: { position: 'absolute', width: 30, height: 30, borderRadius: 15, backgroundColor: 'blue' },
  coin: { position: 'absolute', width: 20, height: 20, borderRadius: 10, backgroundColor: 'gold' },
});
